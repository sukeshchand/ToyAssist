using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.EntityFrameworkCore;

using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.Helpers;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetupView
    {

        List<ExpenseSetup> ExpenseSetups = new List<ExpenseSetup>();
        List<Currency> CurrenciesInUse = new List<Currency>();

        public bool IsPostBack { get; set; }
        public int AccountId { get; set; }

        public ExpenseSetupView()
        {
            SetCulture("en-US");
            AccountId = 1;
        }

        private ExpenseSetupViewModal expenseSetupViewModal = default;

        private async Task OnShowModalClick(ExpenseSetup? data)
        {
            await expenseSetupViewModal.ShowModalAsync(data);
        }


        private void SetCulture(string userCulture)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(userCulture);

        }

        private void LoadData()
        {
            var dataContext = DataContextFactory.Create();
            ExpenseSetups = dataContext.ExpenseSetups
                .Where(x=>x.AccountId == AccountId)
                .Include(i1 => i1.Currency)
                .Include(i2 => i2.Account)
                .ToList();

            CurrenciesInUse = ExpenseSetups.Where(w => w.Currency != null).Select(x => x.Currency).Distinct().ToList();

        }

        protected override void OnInitialized()
        {
            if (!IsPostBack)
            {
                LoadData();
            }
            IsPostBack = true;
        }


        public static DateTime FirstDayOfNextMonth(int nextMonthAfter)
        {
            DateTime today = DateTime.Now;
            if (nextMonthAfter > 1)
            {
                today = FirstDayOfNextMonth(nextMonthAfter - 1);
            }

            // Find the last day of the current month
            DateTime lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

            // Find the first day of the next month
            DateTime firstDayOfNextMonth = lastDayOfMonth.AddDays(1);
            return firstDayOfNextMonth;

        }

        public string GetConversionListForToolTip(Currency baseCurrency, decimal amount)
        {
            var list = GeneralHelper.GetConversionList(baseCurrency, CurrenciesInUse, amount);
            return $" ≈ {string.Join(", ", list)}";
        }

        public static (decimal TotalAmount, decimal TotalTax, bool IsError) CalculateTotalAmountToBePaidInfo(ExpenseSetup? expenseSetupItem)
        {
            if(expenseSetupItem?.StartDate == null || expenseSetupItem.EndDate == null)
            {
                return (0, 0, true);
            }

            var calculationStartDate = (DateTime)expenseSetupItem.StartDate;
            var calculationEndDate = (DateTime)expenseSetupItem.EndDate;

            var amountInfo = CalculateTotalAmount(expenseSetupItem, calculationStartDate, calculationEndDate);

            return (amountInfo.TotalAmount, amountInfo.TotalTax, false);
        }


        public static (decimal TotalAmount, decimal TotalTax, bool IsError) CalculateTotalAmountAlreadyPaidInfo(ExpenseSetup? expenseSetupItem)
        {
            if (expenseSetupItem.StartDate == null)
            {
                return (0, 0, true);
            }

            var calculationStartDate = (DateTime)expenseSetupItem.StartDate;
            var calculationEndDate = DateTime.Now;

            var amountInfo = CalculateTotalAmount(expenseSetupItem, calculationStartDate, calculationEndDate);

            return (amountInfo.TotalAmount, amountInfo.TotalTax, false);
        }

        public static (decimal TotalAmount, decimal TotalTax, bool IsError) CalculateTotalAmountLeftToPayInfo(ExpenseSetup? expenseSetupItem)
        {
            if (expenseSetupItem.EndDate == null)
            {
                return (0, 0, true);
            }

            var calculationStartDate = DateTime.Now;
            var calculationEndDate = (DateTime)expenseSetupItem.EndDate;

            var amountInfo = CalculateTotalAmount(expenseSetupItem, calculationStartDate, calculationEndDate);

            return (amountInfo.TotalAmount, amountInfo.TotalTax, false);
        }

        private static (decimal TotalAmount, decimal TotalTax) CalculateTotalAmount(ExpenseSetup expenseSetupItem, DateTime calculationStartDate, DateTime calculationEndDate)
        {
            var totalAmount = 0m;
            var totalTax = 0m;

            var currentItem = calculationStartDate;

            do
            {
                totalAmount += (decimal)(expenseSetupItem.Amount ?? 0);
                totalTax += (decimal)(expenseSetupItem.TaxAmount ?? 0);
                currentItem = currentItem.AddMonths(1);
            } while (currentItem <= calculationEndDate);

            return (totalAmount, totalTax);
        }

        public List<(string Text, string ToolTipText)> GetRecurringInfo(DateTime? startDate, DateTime? endDate)
        {
            var list = new List<(string, string)>();
            if (startDate != null && endDate != null)
            {
                list.Add(($"{((DateTime)startDate).ToShortDateString()} - {((DateTime)endDate).ToShortDateString()}", string.Empty));

                var totalMonths = ((((DateTime)endDate).Year - ((DateTime)startDate).Year) * 12) + (((DateTime)endDate).Month - ((DateTime)startDate).Month);
                var totalMonthsLeft = ((((DateTime)endDate).Year - DateTime.Now.Year) * 12) + (((DateTime)endDate).Month - DateTime.Now.Month);

                list.Add(($"Total Months: {totalMonths}/{totalMonthsLeft}", "Total Months/Total Months Left"));
            }
            else if (startDate == null && endDate != null)
            {
                list.Add(($"Until {((DateTime)endDate).ToShortDateString()}", $"From not specified, occurrence until {((DateTime)endDate).ToShortDateString()}"));
            }
            else if (startDate != null && endDate == null)
            {
                list.Add(($"From {((DateTime)startDate).ToShortDateString()} until changed", $"End date is not specified, occurrence continues until changed"));
            }
            else if (startDate == null && endDate == null)
            {
                list.Add(("n/a", "No start - end date specified"));
            }
            return list;
        }

    }
}
