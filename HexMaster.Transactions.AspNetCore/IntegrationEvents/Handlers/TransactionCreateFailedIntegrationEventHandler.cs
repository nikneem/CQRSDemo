using System.Threading.Tasks;
using HexMaster.BuildingBlocks.EventBus.Abstractions;
using HexMaster.Transactions.AspNetCore.DataTransferObjects;
using HexMaster.Transactions.AspNetCore.Hubs;
using HexMaster.Transactions.AspNetCore.IntegrationEvents.Events;
using Microsoft.AspNetCore.SignalR;

namespace HexMaster.Transactions.AspNetCore.IntegrationEvents.Handlers
{
    public class TransactionCreateFailedIntegrationEventHandler : IIntegrationEventHandler<TransactionCreateFailedIntegrationEvent>
    {

        private readonly IHubContext<TransactionsHub> _hubContext;

        public async Task Handle(TransactionCreateFailedIntegrationEvent @event)
        {
            var hub = new TransactionsHub(_hubContext);
            await hub.TransactionCreateFailed(new TransactionCreateFailedDto
            {
                ToAccountName = @event.ToAccountName,
                FromAccountName = @event.FromAccountName,
                Amount = @event.Amount,
                Reason = @event.Reason
            });
        }


        public TransactionCreateFailedIntegrationEventHandler(IHubContext<TransactionsHub> hubContext)
        {
            _hubContext = hubContext;
        }
    }
}
