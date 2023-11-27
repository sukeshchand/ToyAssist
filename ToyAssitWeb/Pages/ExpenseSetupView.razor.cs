using System.Globalization;
using System.Text;
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
            else if(startDate == null && endDate != null)
            {
                list.Add(($"Until {((DateTime)endDate).ToShortDateString()}", $"From not specified, occurrence until {((DateTime)endDate).ToShortDateString()}"));
            }
            else if (startDate != null && endDate == null)
            {
                list.Add(($"From {((DateTime)startDate).ToShortDateString()} until changed", $"End date is not specified, occurrence continues until changed"));
            }
            else if (startDate == null && endDate == null)
            {
                list.Add(("n/a","No start - end date specified"));
            }
            return list;
        }

    }
}
