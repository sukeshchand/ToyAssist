namespace ToyAssist.Web.Pages
{

    public class ExpenseSetupModel
    {
        public ExpenseSetupModel()
        {

        }
        // ExpenseSetupId, UserId, ExpenseName, ExpenseDescr, StartDate, EndDate, Amount, CurrencyId, BillGeneratedDay, BillPaymentDay,
        // ExpirationDay, PaymentUrl, AccountProfileUrl
        public int ExpenseSetupId { get; set; }
        public int UserId { get; set; }
            
        public string ExpenseName { get; set; }
        public string? ExpenseDescr { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public int? BillGeneratedDay { get; set; }
        public int? BillPaymentDay { get; set; }
        public int? ExpirationDay { get; set; }
        public string? PaymentUrl { get; set; }
        public string? AccountProfileUrl { get; set; }
    }
}
