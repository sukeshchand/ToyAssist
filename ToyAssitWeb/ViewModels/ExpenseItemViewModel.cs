using ToyAssist.Web.DatabaseModels.Models;

namespace ToyAssist.Web.ViewModels
{
    public class ExpenseItemViewModel
    {
        public ExpenseItemViewModel()
        {
            ExpensePayments = [];
        }
        public ExpenseSetupViewModel? ExpenseSetup { get; set; }
        public List<ExpensePaymentViewModel?> ExpensePayments { get; set; }
        public int ExpensePaymentCurrentIndex { get; set; }

        public int ExpenseSetupId { get; set; }
        public int AccountId { get; set; }
        public string? BillGeneratedText { get; set; }
        public string? BillPaymentText { get; set; }      
    }
}
