using System;
using System.Collections.Generic;
using System.Text;

namespace HexMaster.Transactions.Funcs.IntegrationEvents
{
    public sealed class TransactionCreateFailedIntegrationEvent : IntegrationEventBase
    {

        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }


    }
}
