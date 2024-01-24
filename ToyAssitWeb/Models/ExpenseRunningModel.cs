namespace ToyAssist.Web.Models
{
    public class ExpenseRunningModel
    {
        public int Index { get; set; }
        public DateTime? DateAndTime { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Status { get; set; }
        public bool IsYearBreak { get; set; }
        public bool IsCurrentItem { get; set; }
    }
}
