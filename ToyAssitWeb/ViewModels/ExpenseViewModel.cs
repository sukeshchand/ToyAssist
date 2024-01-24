using ToyAssist.Web.Models;

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

    public class CurrencyGroupViewModel
    {
        public CurrencyGroupViewModel()
        {
            ExpenseItems = new List<ExpenseItemViewModel>();
        }
        public CurrencyModel? Currency { get; set; }
        public List<ExpenseItemViewModel> ExpenseItems { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
    }

    public class ExpenseItemViewModel
    {
        public ExpenseItemViewModel()
        {
            ExpensePayments = [];
        }
        public ExpenseSetupModel? ExpenseSetup { get; set; }
        public List<ExpensePaymentModel?> ExpensePayments { get; set; }
        public int ExpensePaymentCurrentIndex { get; set; }

        public int ExpenseSetupId { get; set; }
        public int AccountId { get; set; }
        public string? BillGeneratedText { get; set; }
        public string? BillPaymentText { get; set; }
    }
}
