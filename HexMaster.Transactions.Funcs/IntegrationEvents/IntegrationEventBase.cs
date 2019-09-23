using System;
using System.Collections.Generic;
using System.Text;

namespace HexMaster.Transactions.Funcs.IntegrationEvents
{
    public abstract class IntegrationEventBase
    {
        protected IntegrationEventBase()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public DateTime CreationDate { get; }
    }
}
