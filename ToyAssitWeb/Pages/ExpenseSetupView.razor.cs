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
        }

        private void LoadData()
        {
            var dataContext = DataContextFactory.Create();
            ExpenseSetups = dataContext.ExpenseSetups
                .Include(i1=>i1.Currency)
                .Include(i2=>i2.Account)
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
            return string.Join(", ", list);
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

    }
}
