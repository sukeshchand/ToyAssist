using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using ToyAssist.Web.DatabaseModels.Models;

using ToyAssist.Web.Factories;
using ToyAssist.Web.Helpers;
using ToyAssist.Web.Mappers.ViewModelRepoMappers;
using ToyAssist.Web.Models;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseMonthly
    {
        public class DataSourceObjects
        {
            public List<CurrencyModel> CurrencyList { get; set; }
            public List<CurrencyModel> CurrenciesInUse { get; set; }
        }

        public DataSourceObjects DataSource { get; set; }

        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        public bool IsPostBack { get; set; }
        public int AccountId { get; set; }


        public ExpenseViewModel ViewModel { get; set; }

        public ExpenseMonthly()
        {
            AccountId = 1;
            IsShowCurrencyConversion = true;
            DataSource = LoadData();
        }


        private ExpenseOverviewPopup expenseOverviewPopup = default;
        private PaymentHandlePopup paymentHandlePopup = default;

        public bool IsShowCurrencyConversion { get; set; }

        private async Task OnShowCurrencyConversion()
        {
            IsShowCurrencyConversion = !IsShowCurrencyConversion;
        }

        private async Task OnClickGoFirst(ExpenseItemViewModel expenseItem)
        {
            expenseItem.ExpensePaymentCurrentIndex = 0;
        }
        private async Task OnClickGoPrevious(ExpenseItemViewModel expenseItem)
        {
            if (expenseItem.ExpensePaymentCurrentIndex > 0)
            {
                expenseItem.ExpensePaymentCurrentIndex = expenseItem.ExpensePaymentCurrentIndex - 1;
            }
        }
        private async Task OnClickGoNext(ExpenseItemViewModel expenseItem)
        {
            if (expenseItem.ExpensePaymentCurrentIndex < expenseItem.ExpensePayments.Count - 1)
            {
                expenseItem.ExpensePaymentCurrentIndex = expenseItem.ExpensePaymentCurrentIndex + 1;
            }
        }
        private async Task OnClickGoLast(ExpenseItemViewModel expenseItem)
        {
            expenseItem.ExpensePaymentCurrentIndex = expenseItem.ExpensePayments.Count - 1;
        }

        private async Task OnClickGoCurrent(ExpenseItemViewModel expenseItem)
        {
            var item = expenseItem.ExpensePayments.Where(x => x.Year >= DateTime.UtcNow.Year && x.Month >= DateTime.UtcNow.Month).OrderBy(o => o.Year).ThenBy(o2 => o2.Month).FirstOrDefault();
            if (item != null)
            {
                expenseItem.ExpensePaymentCurrentIndex = expenseItem.ExpensePayments.IndexOf(item);
            }
        }

        private async Task onHandlePayment(ExpenseItemViewModel expenseItem)
        {
            await paymentHandlePopup.ShowModalAsync(expenseItem);
        }

        private async Task OnViewExpenseItemClick(ExpenseItemViewModel expenseItem)
        {
            await expenseOverviewPopup.ShowModalAsync(expenseItem.ExpenseSetup);
        }


        private DataSourceObjects LoadData()
        {
            var dataSource = new DataSourceObjects();

            var dataContext = DataContextFactory.Create();

            var allExpensePayments = dataContext.ExpensePayments
                .Where(x => x.AccountId == AccountId)
                .ToList();

            var expenseSetups = dataContext.ExpenseSetups
                .Where(x => x.AccountId == AccountId)
                .Include(i1 => i1.Currency)
                .Include(i2 => i2.Account)
                .ToList();

            // Currency List
            dataSource.CurrencyList = dataContext.Currencies.ToList().Select(CurrencyModelMapper.Map).ToList();

            // Currencies in use
            var currencyIdsInUse = expenseSetups.Where(w => w.Currency != null).Select(x => x.Currency.CurrencyId).Distinct().ToList();
            dataSource.CurrenciesInUse = dataSource.CurrencyList.Where(x => currencyIdsInUse.Contains(x.CurrencyId)).ToList();

            ViewModel = BuildExpenseViewModel();
            return dataSource;

            ExpenseViewModel BuildExpenseViewModel()
            {
                var expenseViewModel = new ExpenseViewModel();
                var currencyGroups = expenseSetups.GroupBy(g => g.Currency).Select(g => new { Currency = g.Key, Count = g.Count() }).ToList();
                for (int indexCurrencyGroup = 0; indexCurrencyGroup < currencyGroups.Count; indexCurrencyGroup++)
                {
                    var currencyGroup = new CurrencyGroupViewModel();
                    currencyGroup.Currency = CurrencyModelMapper.Map(currencyGroups[indexCurrencyGroup].Currency);
                    var expenseSetupGropedItems = expenseSetups.Where(x => x.CurrencyId == currencyGroup?.Currency?.CurrencyId).ToList();
                    for (int indexExpenseItem = 0; indexExpenseItem < expenseSetupGropedItems.Count; indexExpenseItem++)
                    {
                        var expenseSetupItem = expenseSetupGropedItems[indexExpenseItem];
                        var expenseItemViewModel = new ExpenseItemViewModel();

                        expenseItemViewModel.ExpenseSetup = ExpenseSetupModelMapper.Map(expenseSetupItem);

                        expenseItemViewModel.AccountId = expenseSetupItem.AccountId;
                        expenseItemViewModel.ExpenseSetupId = expenseSetupItem.ExpenseSetupId;

                        expenseItemViewModel.BillGeneratedText = GetBillGeneratedText(expenseItemViewModel);
                        expenseItemViewModel.BillPaymentText = GetBillPaymentText(expenseItemViewModel);
                        expenseItemViewModel.ExpensePayments = GetExpensePayments(allExpensePayments, expenseSetupItem);

                        currencyGroup.ExpenseItems.Add(expenseItemViewModel);
                    }
                    currencyGroup.TotalAmount = expenseSetupGropedItems.Sum(x => x.Amount ?? 0);
                    currencyGroup.TotalTaxAmount = expenseSetupGropedItems.Sum(x => x.TaxAmount ?? 0);
                    expenseViewModel.CurrencyGroups.Add(currencyGroup);
                }
                return expenseViewModel;
            }
        }

        private async void OnPaymentDataUpdatedEvent(ExpenseItemViewModel data)
        {
            var isUpdated = false;
            for (int indexCurrencyGroup = 0; indexCurrencyGroup < ViewModel.CurrencyGroups.Count; indexCurrencyGroup++)
            {
                for (int indexItem = 0; indexItem < ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems.Count; indexItem++)
                {
                    if (ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems[indexItem].ExpenseSetupId == data.ExpenseSetupId)
                    {
                        ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems[indexItem] = data;
                        isUpdated = true;
                        break;
                    }
                }
                if (isUpdated) break;
            }
            if (isUpdated)
            {
                StateHasChanged();
                await Task.Delay(1);
            }
        }



        private static List<ExpensePaymentModel?> GetExpensePayments(List<ExpensePayment> allExpensePayments, ExpenseSetup expenseSetup)
        {
            var expenseSetupViewModel = ExpenseSetupModelMapper.Map(expenseSetup);
            var runningExpensePayments = GeneralHelper.GetExpenseRunningList(expenseSetupViewModel);
            var expensePayments = new List<ExpensePaymentModel>();
            for (int i = 0; i < runningExpensePayments.Count; i++)
            {
                var runningExpensePayment = runningExpensePayments[i];
                var expensePaymentExist = allExpensePayments.FirstOrDefault(x => x.AccountId == expenseSetup.AccountId
                                                                        && x.ExpenseSetupId == expenseSetup.ExpenseSetupId
                                                                        && x.Month == ((DateTime)runningExpensePayment.DateAndTime).Month
                                                                        && x.Year == ((DateTime)runningExpensePayment.DateAndTime).Year
                                                                        );
                ExpensePaymentModel? expensePayment = null;
                if (expensePaymentExist == null)
                {
                    expensePayment = new ExpensePaymentModel()
                    {
                        Index = runningExpensePayment.Index,
                        AccountId = expenseSetup.AccountId,
                        ExpenseSetupId = expenseSetup.ExpenseSetupId,
                        Amount = runningExpensePayment.Amount,
                        Tax = runningExpensePayment.Tax,
                        Month = ((DateTime)runningExpensePayment.DateAndTime).Month,
                        Year = ((DateTime)runningExpensePayment.DateAndTime).Year,
                    };
                }
                else
                {
                    expensePayment = ExpensePaymentModelMapper.Map(expensePaymentExist);
                    expensePayment.Index = runningExpensePayment.Index;
                }

                expensePayments.Add(expensePayment);
            }
            return expensePayments;
        }

        private static string? GetBillGeneratedText(ExpenseItemViewModel expenseItemViewModel)
        {
            var remindDaysBillGenerated = GetRemindDays(expenseItemViewModel.ExpenseSetup.BillGeneratedDay);
            if (remindDaysBillGenerated != null)
            {
                return $"Next Invoice will generate in {remindDaysBillGenerated} days";
            }
            return null;
        }

        private static string? GetBillPaymentText(ExpenseItemViewModel expenseItemViewModel)
        {
            var remindDaysBillPayment = GetRemindDays(expenseItemViewModel.ExpenseSetup.BillPaymentDay);
            if (remindDaysBillPayment != null)
            {
                return $"Next Invoice should pay in {remindDaysBillPayment} days";
            }
            return null;
        }

        private static int? GetRemindDays(int? reminderDay)
        {
            var remindBeforeDays = 7;
            if (reminderDay == null)
            {
                return null;
            }

            var reminderDateThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, (int)reminderDay);
            var alertFromDateForThisMonth = reminderDateThisMonth.AddDays(-1 * remindBeforeDays);
            if (DateTime.Now.Date >= alertFromDateForThisMonth.Date && DateTime.Now.Date <= reminderDateThisMonth.Date)
            {
                return (reminderDateThisMonth.Date - DateTime.Now.Date).Days;
            }

            var reminderDateNextMonth = reminderDateThisMonth.AddMonths(1);
            var alertFromDateForNextMonth = reminderDateThisMonth.AddDays(-1 * remindBeforeDays);
            if (DateTime.Now.Date >= alertFromDateForNextMonth.Date && DateTime.Now.Date <= reminderDateNextMonth.Date)
            {
                return (reminderDateNextMonth.Date - DateTime.Now.Date).Days;
            }

            return null;
        }
    }
}
