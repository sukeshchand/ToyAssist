namespace ToyAssist.Web.Pages
{
    public partial class Index
    {
        public class IncomeExpenseSetup 
        {
            public IncomeExpenseSetup()
            {

            }
            public int IncomeExpenseSetupId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int IncomeExpenseType { get; set; }
            public double Amount { get; set; }
            public string Currency { get; set; }
            public string Descr { get; set; }
        }

    }
}
