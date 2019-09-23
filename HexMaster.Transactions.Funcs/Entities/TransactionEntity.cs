using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace HexMaster.Transactions.Funcs.Entities
{
    public sealed class TransactionEntity : TableEntity
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
