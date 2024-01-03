using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.EventArgsModels;
using ToyAssist.Web.Factories;
using ToyAssist.Web.Mappers.ViewModelRepoMappers;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Pages
{


    public partial class ExpenseMonthlyView
    {

        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        //List<ExpensePayment> ExpensePayments = new List<ExpensePayment>();
        public bool IsPostBack { get; set; }
        public int AccountId { get; set; }
        public List<CurrencyViewModel> CurrenciesInUse { get; set; }
        public List<CurrencyViewModel> CurrencyList { get; set; }

        public ExpenseViewModel ViewModel { get; set; }

        public ExpenseMonthlyView()
        {
            AccountId = 1;
            IsShowCurrencyConversion = true;
            LoadData();
        }


        private ExpenseSetupViewModal expenseSetupViewModal = default;
        private ExpenseMonthlyHandlePaymentModal expenseMonthlyHandlePaymentModal = default;

        public bool IsShowCurrencyConversion { get; set; }

        private async Task OnShowCurrencyConversion()
        {
            IsShowCurrencyConversion = !IsShowCurrencyConversion;
        }

        private async Task onHandlePayment(ExpenseItemViewModel expenseItem)
        {
            await expenseMonthlyHandlePaymentModal.ShowModalAsync(expenseItem);
        }

        private async Task OnViewExpenseItemClick(ExpenseItemViewModel expenseItem)
        {
            await expenseSetupViewModal.ShowModalAsync(expenseItem.ExpenseSetup);
        }

        private void LoadData()
        {
            var dataContext = DataContextFactory.Create();

            var expenseSetups = dataContext.ExpenseSetups
                .Where(x => x.AccountId == AccountId)
                .Include(i1 => i1.Currency)
                .Include(i2 => i2.Account)
                .ToList();

            var expensePayments = dataContext.ExpensePayments
                .Where(x => x.AccountId == AccountId)
                .ToList();

            //ExpensePayments = expensePayments;

            // Currency List
            CurrencyList = dataContext.Currencies.ToList().Select(CurrencyViewModelMapper.Map).ToList();

            // Currencies in use
            var currencyIdsInUse = expenseSetups.Where(w => w.Currency != null).Select(x => x.Currency.CurrencyId).Distinct().ToList();
            CurrenciesInUse = CurrencyList.Where(x => currencyIdsInUse.Contains(x.CurrencyId)).ToList();


            ViewModel = BuildViewModel(expenseSetups, expensePayments);

        }

        private async void OnPaymentDataUpdatedEvent(ExpenseItemViewModel data)
        {
            //e.ExpenseItemViewModel
            //LoadData();
            //return;
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

        private ExpenseViewModel BuildViewModel(List<ExpenseSetup> expenseSetups, List<ExpensePayment> expensePayments)
        {
            var expenseViewModel = new ExpenseViewModel();
            var currencyGroups = expenseSetups.GroupBy(g => g.Currency).Select(g => new { Currency = g.Key, Count = g.Count() }).ToList();
            for (int indexCurrencyGroup = 0; indexCurrencyGroup < currencyGroups.Count; indexCurrencyGroup++)
            {
                var currencyGroup = new CurrencyGroupViewModel();
                currencyGroup.Currency = CurrencyViewModelMapper.Map(currencyGroups[indexCurrencyGroup].Currency);
                var expenseItems = expenseSetups.Where(x => x.CurrencyId == currencyGroup?.Currency?.CurrencyId).ToList();
                for (int indexExpenseItem = 0; indexExpenseItem < expenseItems.Count; indexExpenseItem++)
                {
                    var expenseItem = expenseItems[indexExpenseItem];
                    var expenseItemViewModel = new ExpenseItemViewModel();

                    expenseItemViewModel.ExpenseSetup = ExpenseSetupViewModelMapper.Map(expenseItem);

                    expenseItemViewModel.AccountId = expenseItem.AccountId;
                    expenseItemViewModel.ExpenseSetupId = expenseItem.ExpenseSetupId;

                    expenseItemViewModel.BillGeneratedText = GetBillGeneratedText(expenseItemViewModel);
                    expenseItemViewModel.BillPaymentText = GetBillPaymentText(expenseItemViewModel);

                    expenseItemViewModel.ExpensePayments = expensePayments
                        .Where(x => x.ExpenseSetupId == expenseItem.ExpenseSetupId)
                        .Select(x => ExpensePaymentViewModelMapper.Map(x, x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month))
                        .ToList();

                    //----------------
                    currencyGroup.ExpenseItems.Add(expenseItemViewModel);
                }
                currencyGroup.TotalAmount = expenseItems.Sum(x => x.Amount ?? 0);
                currencyGroup.TotalTaxAmount = expenseItems.Sum(x => x.TaxAmount ?? 0);
                expenseViewModel.CurrencyGroups.Add(currencyGroup);
            }
            return expenseViewModel;
        }

        private static string? GetBillGeneratedText(ExpenseItemViewModel expenseItemViewModel)
        {
            var remindDaysBillGenerated = GetRemindDays(expenseItemViewModel.ExpenseSetup.BillGeneratedDay);
            if (remindDaysBillGenerated != null)
            {
                return $"Bill will generate in {remindDaysBillGenerated} days";
            }
            return null;
        }

        private static string? GetBillPaymentText(ExpenseItemViewModel expenseItemViewModel)
        {
            var remindDaysBillPayment = GetRemindDays(expenseItemViewModel.ExpenseSetup.BillPaymentDay);
            if (remindDaysBillPayment != null)
            {
                return $"Bill payment in {remindDaysBillPayment} days";
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
