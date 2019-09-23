namespace HexMaster.Transactions.AspNetCore.DataTransferObjects
{
    public sealed class TransactionCreateFailedDto
    {
        public string FromAccountName { get; set; }
        public string ToAccountName { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
    }
}
