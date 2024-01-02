
using ToyAssist.Web.DatabaseModels.Models;

namespace ToyAssist.Web.ViewModels
{
    public class CurrencyGroupViewModel
    {
        public CurrencyGroupViewModel()
        {
            ExpenseItems = new List<ExpenseItemViewModel>();
        }
        public CurrencyViewModel? Currency { get; set; }
        public List<ExpenseItemViewModel> ExpenseItems { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
    }
}
