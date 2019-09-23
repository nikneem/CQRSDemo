using System;

namespace HexMaster.Transactions.Funcs.IntegrationEvents
{
    public sealed class TransactionCreatedIntegrationEvent : IntegrationEventBase
    {

        public Guid? TransactionId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal NewBalance { get; set; }

    }
}
