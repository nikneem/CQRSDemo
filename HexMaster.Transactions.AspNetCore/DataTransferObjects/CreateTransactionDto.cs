using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexMaster.Transactions.AspNetCore.DataTransferObjects
{
    public sealed class CreateTransactionDto
    {

        public string FromAccountNumber { get; set; }
        public string FromAccountHolder { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToAccountHolder { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTimeOffset TransactionOn { get; set; }

    }
}
