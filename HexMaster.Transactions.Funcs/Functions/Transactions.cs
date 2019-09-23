using System;
using System.Text;
using System.Threading.Tasks;
using HexMaster.Transactions.Funcs.DataTransferObjects;
using HexMaster.Transactions.Funcs.Entities;
using HexMaster.Transactions.Funcs.IntegrationEvents;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HexMaster.Transactions.Funcs.Functions
{
    public sealed class Transactions
    {

        private const string IntegrationEventSufix = "IntegrationEvent";


        [FunctionName("CreateTransaction")]
        public static async void CreateTransaction(
            [ServiceBusTrigger("transactions", Connection = "AzureServiceBus")] string message,
            [ServiceBus("bank", Connection = "AzureServiceBus", EntityType = EntityType.Topic)] IAsyncCollector<Message> serviceBusTopic,
            [Table("transactions")] IAsyncCollector<TransactionEntity> table,
            ILogger log)
        {

            var transaction = JsonConvert.DeserializeObject<CreateTransactionDto>(message);
            if (transaction != null)
            {
                if (transaction.Amount > 100)
                {
                    var integrationEvent = new TransactionCreateFailedIntegrationEvent
                    {
                        Amount = transaction.Amount,
                        FromAccountName = transaction.FromAccountHolder,
                        ToAccountName = transaction.ToAccountHolder,
                        Reason = "Maximum transaction amount is 100"
                    };
                    await SendServicebusMessage(integrationEvent, serviceBusTopic);
                }
                else
                {
                    var transactionEntity = new TransactionEntity
                    {
                        PartitionKey = "transaction",
                        RowKey = Guid.NewGuid().ToString(),
                        FromAccountNumber = transaction.FromAccountNumber,
                        FromAccountHolder = transaction.FromAccountHolder,
                        ToAccountNumber = transaction.ToAccountNumber,
                        ToAccountHolder = transaction.ToAccountHolder,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        TransactionOn = transaction.TransactionOn,
                        Timestamp = DateTimeOffset.UtcNow
                    };
                    await table.AddAsync(transactionEntity);
                    var integrationEvent = new TransactionCreatedIntegrationEvent
                    {
                        TransactionId = Guid.Parse( transactionEntity.RowKey),
                        FromAccountName= transaction.FromAccountHolder,
                        ToAccountName= transaction.ToAccountHolder,
                        NewBalance = 3581.53M
                    };
                    await SendServicebusMessage(integrationEvent, serviceBusTopic);
                }

                await serviceBusTopic.FlushAsync();
            }

        }

        private static async Task SendServicebusMessage<T>(T message, IAsyncCollector<Message> serviceBusTopic)
        {
            var eventName = message.GetType().Name.Replace(IntegrationEventSufix, "");
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var serviceBusErrorMessage = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };
            await serviceBusTopic.AddAsync(serviceBusErrorMessage);
        }

    }
}
