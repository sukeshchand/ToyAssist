using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Enums;

namespace ToyAssist.Web.ViewModels
{
    public class ExpensePaymentViewModel
    {
        public long ExpensePaymentId { get; set; }

        public int ExpenseSetupId { get; set; }

        public int AccountId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public DateTime? PaymentDoneDate { get; set; }

        public ExpensePaymentStatusEnum ExpensePaymentStatus { get; set; }

        public string? Descr { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
