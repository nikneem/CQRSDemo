using System;
using HexMaster.BuildingBlocks.EventBus.Events;

namespace HexMaster.Transactions.AspNetCore.IntegrationEvents.Events
{
    public sealed class TransactionCreatedIntegrationEvent : IntegrationEvent
    {

        public Guid? TransactionId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal NewBalance { get; set; }

    }
}
