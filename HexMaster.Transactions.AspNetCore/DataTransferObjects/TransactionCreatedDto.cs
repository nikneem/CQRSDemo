using System;

namespace HexMaster.Transactions.AspNetCore.DataTransferObjects
{
    public sealed class TransactionCreatedDto
    {
        public Guid? TransactionId { get; set; }
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal NewBalance { get; set; }
    }
}
