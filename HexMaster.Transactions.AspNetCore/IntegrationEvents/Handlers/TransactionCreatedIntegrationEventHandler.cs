using System.Threading.Tasks;
using HexMaster.BuildingBlocks.EventBus.Abstractions;
using HexMaster.Transactions.AspNetCore.DataTransferObjects;
using HexMaster.Transactions.AspNetCore.Hubs;
using HexMaster.Transactions.AspNetCore.IntegrationEvents.Events;
using Microsoft.AspNetCore.SignalR;

namespace HexMaster.Transactions.AspNetCore.IntegrationEvents.Handlers
{
    public class TransactionCreatedIntegrationEventHandler : IIntegrationEventHandler<TransactionCreatedIntegrationEvent>
    {
        private readonly IHubContext<TransactionsHub> _hubContext;

        public async Task Handle(TransactionCreatedIntegrationEvent @event)
        {
            var hub = new TransactionsHub(_hubContext);
            await hub.TransactionCreated(new TransactionCreatedDto
            {
                ToAccountName = @event.ToAccountName,
                FromAccountName = @event.FromAccountName,
                NewBalance = @event.NewBalance,
                TransactionId = @event.TransactionId
            });
        }


        public TransactionCreatedIntegrationEventHandler(IHubContext<TransactionsHub> hubContext)
        {
            _hubContext = hubContext;
        }

    }
}
