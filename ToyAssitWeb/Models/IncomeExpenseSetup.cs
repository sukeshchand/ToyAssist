namespace ToyAssist.Web.Pages
{

    public class IncomeExpenseSetup
    {
        public IncomeExpenseSetup()
        {

        }
        public int IncomeExpenseSetupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public int IncomeExpenseType { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Descr { get; set; }
        public string? PaymentUrl { get; set; }
        public string? AccountLogInUrl { get; set; }
        // IncomeExpenseSetupId, StartDate, EndDate, IncomeExpenseType, Amount, Currency, Descr, NextPaymentDate, NextBillingDate, PaymentUrl, AccountLogInUrl
    }


}
