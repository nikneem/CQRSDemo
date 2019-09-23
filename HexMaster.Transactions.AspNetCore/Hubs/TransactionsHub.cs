using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HexMaster.Transactions.AspNetCore.DataTransferObjects;
using Microsoft.AspNetCore.SignalR;

namespace HexMaster.Transactions.AspNetCore.Hubs
{
    public class TransactionsHub : Hub
    {

        protected IHubContext<TransactionsHub> _context;

        public async Task TransactionCreated(TransactionCreatedDto transaction)
        {
            await _context.Clients.All.SendCoreAsync("transaction-created", new[] { transaction });
        }
        public async Task TransactionCreateFailed(TransactionCreateFailedDto info)
        {
            await _context.Clients.All.SendCoreAsync("transaction-failed", new[] { info });
        }

        public TransactionsHub(IHubContext<TransactionsHub> context)
        {
            _context = context;
        }

    }
}
