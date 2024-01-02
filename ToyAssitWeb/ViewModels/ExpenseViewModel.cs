namespace ToyAssist.Web.ViewModels
{
    public class ExpenseViewModel
    {
        public List<CurrencyGroupViewModel> CurrencyGroups { get; set; }
        public ExpenseViewModel()
        {
            CurrencyGroups = new List<CurrencyGroupViewModel>();
        }
    }
}
