using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Pages
{
    public class ExpenseViewModel
    {
        public List<CurrencyGroup> CurrencyGroups { get; set; }
         public ExpenseViewModel()
        {
            CurrencyGroups = new List<CurrencyGroup>(); 
        }
    }

    public class CurrencyGroup
    {
        public CurrencyGroup() 
        {
            ExpenseItems = new List<ExpenseItemViewModel>();
        }
        public Currency? Currency { get; set; }
        public List<ExpenseItemViewModel> ExpenseItems { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
    }

    public class ExpenseItemViewModel
    {
        public int ExpenseSetupId { get; set; }

        public int AccountId { get; set; }

        public string ExpenseName { get; set; } = null!;

        public string? ExpenseDescr { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Amount { get; set; }

        public decimal? TaxAmount { get; set; }

        public int? CurrencyId { get; set; }

        public int? BillGeneratedDay { get; set; }

        public int? BillPaymentDay { get; set; }

        public int? ExpirationDay { get; set; }

        public string? PaymentUrl { get; set; }

        public string? AccountProfileUrl { get; set; }
    }

    public partial class ExpenseMonthlyView
    {

        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        public bool IsPostBack { get; set; }
        public int AccountId { get; set; }
        public List<Currency> CurrenciesInUse { get; set; }

        public ExpenseViewModel ViewModel { get; set; }

        public ExpenseMonthlyView()
        {
            AccountId = 1;
            LoadData();
        }

        private void LoadData()
        {
            var dataContext = DataContextFactory.Create();

            var expenseSetups = dataContext.ExpenseSetups
                .Where(x => x.AccountId == AccountId)
                .Include(i1 => i1.Currency)
                .Include(i2 => i2.Account)
                .ToList();

            CurrenciesInUse = expenseSetups.Where(w => w.Currency != null).Select(x => x.Currency).Distinct().ToList();

            ViewModel = BuildViewModel(expenseSetups);
        }

        public bool IsShowCurrencyConversion { get; set; }

        private async Task OnShowCurrencyConversion()
        {
            IsShowCurrencyConversion = !IsShowCurrencyConversion;
        }

        private ExpenseViewModel BuildViewModel(List<ExpenseSetup> expenseSetups)
        {
           var expenseViewModel = new ExpenseViewModel();
            var currencyGroups = expenseSetups.GroupBy(g => g.Currency).Select(g => new { Currency = g.Key, Count = g.Count() }).ToList();
            for (int indexCurrencyGroup = 0; indexCurrencyGroup < currencyGroups.Count; indexCurrencyGroup++)
            {
                var currencyGroup = new CurrencyGroup();
                currencyGroup.Currency = currencyGroups[indexCurrencyGroup].Currency;
                var expenseItems = expenseSetups.Where(x => x.CurrencyId == currencyGroup?.Currency?.CurrencyId).ToList();
                for (int indexExpenseItem = 0; indexExpenseItem < expenseItems.Count; indexExpenseItem++)
                {
                    var expenseItem = expenseItems[indexExpenseItem];
                    var expenseItemViewModel = new ExpenseItemViewModel();

                    expenseItemViewModel.ExpenseName = expenseItem.ExpenseName ?? string.Empty;
                    expenseItemViewModel.Amount = expenseItem.Amount;
                    expenseItemViewModel.TaxAmount = expenseItem.TaxAmount;

                    currencyGroup.ExpenseItems.Add(expenseItemViewModel);
                }
                currencyGroup.TotalAmount = expenseItems.Sum(x => x.Amount ?? 0);
                currencyGroup.TotalTaxAmount = expenseItems.Sum(x => x.TaxAmount ?? 0);
                expenseViewModel.CurrencyGroups.Add(currencyGroup);
            }
            return expenseViewModel;
        }
    }
}
