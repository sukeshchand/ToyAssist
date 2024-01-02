using ToyAssist.Web.DatabaseModels.Models;

namespace ToyAssist.Web.ViewModels
{
    public class ExpensePaymentViewModel
    {
        public ExpensePayment ExpensePayment { get; set; }
        public bool IsCurrent { get; set; }
    }
}
