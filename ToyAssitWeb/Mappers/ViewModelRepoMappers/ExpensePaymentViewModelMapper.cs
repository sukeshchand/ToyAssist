using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class ExpensePaymentViewModelMapper
    {
        public static ExpensePaymentViewModel? Map(ExpensePayment? item, bool isCurrent = false)
        {
            if (item == null)
            {
                return null;
            }

            return new ExpensePaymentViewModel()
            {
                AccountId = item.AccountId,
                ExpensePaymentId = item.ExpensePaymentId,
                PaymentDoneDate = item.PaymentDoneDate,
                Year = item.Year,
                CreatedDateTime = item.CreatedDateTime,
                Descr = item.Descr,
                ExpensePaymentStatus = item.ExpensePaymentStatus,
                ExpenseSetupId = item.ExpenseSetupId,
                Month = item.Month,
                Amount = item.Amount
            };
        }
    }
}
