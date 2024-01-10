using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Mappers.ViewModelRepoMappers
{
    public static class ExpenseSetupViewModelMapper
    {
        public static ExpenseSetupViewModel? Map(ExpenseSetup? item)
        {
            if (item == null)
            {
                return null;
            }           

            return new ExpenseSetupViewModel()
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
                Currency = CurrencyViewModelMapper.Map(item.Currency)
            };
        }
    }
}
