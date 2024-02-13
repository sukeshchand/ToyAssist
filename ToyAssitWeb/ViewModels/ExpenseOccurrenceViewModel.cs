using ToyAssist.Web.Models;

namespace ToyAssist.Web.ViewModels
{
    public class ExpenseOccurrenceViewModel
    {
        public List<CurrencyModel> CurrencyList { get; set; }
        public List<CurrencyModel> CurrenciesInUse { get; set; }

        public List<CurrencyGroupViewModel> CurrencyGroups { get; set; }
        public ExpenseOccurrenceViewModel()
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
