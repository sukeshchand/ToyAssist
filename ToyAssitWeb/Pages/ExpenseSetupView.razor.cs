using System;
using System.Globalization;
using System.Text;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetupView
    {
        List<ExpenseSetup> ExpenseSetups = new List<ExpenseSetup>();
        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        public bool IsPostBack { get; set; }

        public ExpenseSetupView()
        {
            SetCulture("en-US");
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
                // .Where(x=>x.CurrencyId == 1) // this is for testing
                .Include(i1 => i1.Currency)
                .Include(i2 => i2.Account)
                .ToList();
            CurrencyConversionRates = dataContext.CurrencyConversionRates.ToList();
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

        public string GetConversionListForToolTip(Currency baseCurrency, double amount)
        {
            var list = GetConversionList(baseCurrency, amount);
            return $" ≈ {string.Join(", ", list)}";
        }

        public List<string> GetConversionList(Currency baseCurrency, double amount)
        {
            var currenciesInUse = ExpenseSetups.Select(x => x.Currency).Distinct().ToList();
            var conversionList = new List<string>();
            foreach (var currency in currenciesInUse)
            {
                var conversionRate = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == baseCurrency.CurrencyId && x.ToCurrencyId == currency!.CurrencyId);
                if (conversionRate != null)
                {
                    conversionList.Add($"{currency.CurrencyCode} {(int)(amount * (double)conversionRate.ConversionRate)}");
                }
                else
                {
                    var conversionRateReverse = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == currency.CurrencyId && x.ToCurrencyId == baseCurrency.CurrencyId);
                    if (conversionRateReverse != null)
                    {
                        conversionList.Add($"{currency.CurrencyCode} {(int)(amount / (double)conversionRateReverse.ConversionRate)}");
                    }
                }
            }
            return conversionList;
        }

        private List<(int Index, DateTime DateAndTime, double? Amount, double? Tax)> GetExpenseRunningList()
        {
            return new List<(int Index, DateTime DateAndTime, double? Amount, double? Tax)>();
        }

        public class ExpenseRunningModel
        {
            public int Index { get; set; }
            public DateTime? DateAndTime { get; set; }
            public double? Amount { get; set; }
            public double? Tax { get; set; }
            public double? TotalAmount { get; set; }
            public string? Status { get; set; }
            public bool IsYearBreak { get; set; }
            public bool IsCurrentItem { get; set; }
        }

        private List<ExpenseRunningModel> GetExpenseRunningList1(ExpenseSetup expenseSetup)
        {
            var list = new List<ExpenseRunningModel>();
            if (@ModalData?.StartDate == null || @ModalData.EndDate == null) return list;

            var startDate = (DateTime)@ModalData.StartDate;
            var currentDate = (DateTime)@ModalData.StartDate;
            var endDate = (DateTime)@ModalData.EndDate;
            var index = 0;
            var currentItemAssigned = false;

            do
            {
                var listItem = new ExpenseRunningModel
                {
                    Index = index + 1,
                    DateAndTime = currentDate,
                    Amount = expenseSetup?.Amount ?? 0,
                    Status = currentDate > DateTime.Now ? "Pending" : "Paid",
                    TotalAmount = (expenseSetup?.Amount ?? 0) + (expenseSetup?.TaxAmount ?? 0)
                };
                if ((index + 1) % 12 == 0)
                {
                    listItem.IsYearBreak = true;
                }

                if ((!currentItemAssigned && index > 0 && list[index - 1].Status == "Paid" && listItem.Status == "Pending") ||
                        (listItem.Status == "Pending" && index == 0))
                {
                    listItem.IsCurrentItem = true;
                    currentItemAssigned = true;
                }

                // ------------------------ Calculations end ------------------------
                list.Add(listItem);
                currentDate = currentDate.AddMonths(1);
                index++;

            } while (endDate > currentDate);
            return list;
        }

        private (int Index, DateTime DateAndTime, double? Amount, double? Tax, string? Status) GetExpenseRunningItem()
        {
            (int Index, DateTime DateAndTime, double? Amount, double? Tax, string? Status) item = new();
            return item;
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
