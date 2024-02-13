using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class ExpenseSetupModelMapper
    {
        public static ExpenseSetupModel? Map(ExpenseSetup? item)
        {
            if (item == null)
            {
                return null;
            }           

            return new ExpenseSetupModel()
            {
                ExpenseSetupId = item.ExpenseSetupId,
                AccountId = item.AccountId,
                ExpenseName = item.ExpenseName,
                ExpenseDescr = item.ExpenseDescr,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Amount = item.Amount,
                TaxAmount = item.TaxAmount,
                CurrencyId = item.CurrencyId,
                BillGeneratedDay = item.BillGeneratedDay,
                BillPaymentDay = item.BillPaymentDay,
                ExpirationDay = item.ExpirationDay,
                PaymentUrl = item.PaymentUrl,
                AccountProfileUrl = item.AccountProfileUrl,
                Currency = CurrencyModelMapper.Map(item.Currency)
            };
        }
    }
}
