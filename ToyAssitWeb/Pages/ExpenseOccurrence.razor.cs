using System;
using System.Collections.Generic;
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

    public partial class ExpenseOccurrence
    {
        public ExpenseOccurrenceViewModel ViewModel { get; set; }

        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        public bool IsPostBack { get; set; }
        public int AccountId { get; set; }


        public ExpenseOccurrence()
        {
            AccountId = 1;
            IsShowCurrencyConversion = true;
            ViewModel = LoadData();
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

        private async Task onHandlePayment(ExpenseItemViewModel expenseItem, ExpensePaymentModel? expensePayment)
        {
            await paymentHandlePopup.ShowModalAsync(expenseItem, expensePayment);
        }

        private async Task OnViewExpenseItemClick(ExpenseItemViewModel expenseItem)
        {
            await expenseOverviewPopup.ShowModalAsync(expenseItem.ExpenseSetup);
        }


        private ExpenseOccurrenceViewModel LoadData()
        {
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
            var viewModel = new ExpenseOccurrenceViewModel();
            viewModel.CurrencyList = dataContext.Currencies.ToList().Select(CurrencyModelMapper.Map).ToList();

            // Currencies in use
            var currencyIdsInUse = expenseSetups.Where(w => w.Currency != null).Select(x => x.Currency.CurrencyId).Distinct().ToList();
            viewModel.CurrenciesInUse = viewModel.CurrencyList.Where(x => currencyIdsInUse.Contains(x.CurrencyId)).ToList();

            viewModel.CurrencyGroups = BuildCurrencyGroups(expenseSetups, allExpensePayments);
            return viewModel;
           
        }

        private static List<CurrencyGroupViewModel> BuildCurrencyGroups(List<ExpenseSetup> expenseSetups, List<ExpensePayment> allExpensePayments)
        {
            var currencyGroups = new List<CurrencyGroupViewModel>();
            var currencyGroupsWithCount = expenseSetups.GroupBy(g => g.Currency).Select(g => new { Currency = g.Key, Count = g.Count() }).ToList();
            for (int indexCurrencyGroup = 0; indexCurrencyGroup < currencyGroupsWithCount.Count; indexCurrencyGroup++)
            {
                var currencyGroup = new CurrencyGroupViewModel();
                currencyGroup.Currency = CurrencyModelMapper.Map(currencyGroupsWithCount[indexCurrencyGroup].Currency);
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
                    expenseItemViewModel.ExpensePayments = GeneralHelper.BuildExpensePayments(allExpensePayments, expenseSetupItem);

                    currencyGroup.ExpenseItems.Add(expenseItemViewModel);
                }
                currencyGroup.TotalAmount = expenseSetupGropedItems.Sum(x => x.Amount ?? 0);
                currencyGroup.TotalTaxAmount = expenseSetupGropedItems.Sum(x => x.TaxAmount ?? 0);
                currencyGroups.Add(currencyGroup);
            }
            return currencyGroups;
        }

        private async void OnPaymentDataUpdatedEvent(ExpensePaymentModel data)
        {
            var isUpdated = false;
            for (int indexCurrencyGroup = 0; indexCurrencyGroup < ViewModel.CurrencyGroups.Count; indexCurrencyGroup++)
            {
                for (int indexExpenseItem = 0; indexExpenseItem < ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems.Count; indexExpenseItem++)
                {
                    if (ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems[indexExpenseItem].ExpenseSetupId == data.ExpenseSetupId)
                    {
                        var expenseItem = ViewModel.CurrencyGroups[indexCurrencyGroup].ExpenseItems[indexExpenseItem];
                        for (int indexPayment = 0; indexPayment < expenseItem.ExpensePayments.Count; indexPayment++)
                        {
                            if (expenseItem.ExpensePayments[indexPayment].ExpenseSetupId == data.ExpenseSetupId && expenseItem.ExpensePayments[indexPayment].AccountId == data.AccountId && expenseItem.ExpensePayments[indexPayment].Year == data.Year && expenseItem.ExpensePayments[indexPayment].Month == data.Month)
                            {
                                var index = expenseItem.ExpensePayments[indexPayment].Index;
                                expenseItem.ExpensePayments[indexPayment] = data;
                                expenseItem.ExpensePayments[indexPayment].Index = index;
                                isUpdated = true;
                                break;
                            }
                        }
                        if (isUpdated) break;
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
            DateTime reminderDateThisMonth = CreateReminderDateOfThisMonth(reminderDay);
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

        private static DateTime CreateReminderDateOfThisMonth(int? reminderDay)
        {
            DateTime dt;
            try
            {
                if(DateTime.TryParse($"{DateTime.Now.Year}-{DateTime.Now.Month}-{reminderDay}", out dt))
                {
                    return dt;
                }
            }
            catch (Exception ex)
            {
                
            }
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }
    }
}
