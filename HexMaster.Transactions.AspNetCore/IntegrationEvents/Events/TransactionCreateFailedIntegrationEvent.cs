using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexMaster.BuildingBlocks.EventBus.Events;

namespace HexMaster.Transactions.AspNetCore.IntegrationEvents.Events
{
    public class TransactionCreateFailedIntegrationEvent : IntegrationEvent
    {
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
