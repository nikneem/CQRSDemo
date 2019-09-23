using System;

namespace HexMaster.Transactions.Funcs.DataTransferObjects
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
