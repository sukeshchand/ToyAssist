using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class ExpensePaymentModelMapper
    {
        public static ExpensePaymentModel? Map(ExpensePayment? item, bool isCurrent = false)
        {
            if (item == null)
            {
                return null;
            }

            return new ExpensePaymentModel()
            {
                AccountId = item.AccountId,
                ExpensePaymentId = item.ExpensePaymentId,
                PaymentDoneDate = item.PaymentDoneDate,
                Year = item.Year,
                CreatedDateTime = item.CreatedDateTime,
                Descr = item.Descr,
                PaymentStatus = item.PaymentStatus,
                ExpenseSetupId = item.ExpenseSetupId,
                Month = item.Month,
                Amount = item.Amount
            };
        }
    }
}
