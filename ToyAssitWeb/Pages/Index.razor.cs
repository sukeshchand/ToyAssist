//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.EntityFrameworkCore;

//using ToyAssist.Web.DatabaseModels.Models;
//using ToyAssist.Web.Helpers;

//namespace ToyAssist.Web.Pages
//{

//    public partial class Index
//    {
//        public string ConnectionString;
//        private List<string> results = new List<string>();

//        public IConfiguration _configuration { get; }

//        List<IncomeExpenseSetup> IncomeExpenseSetups = new List<IncomeExpenseSetup>();
//        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
//        private readonly DataContext _dataContext;

//        public string Month1ButtonLabel { get; set; }
//        public string Month2ButtonLabel { get; set; }
//        public string Month3ButtonLabel { get; set; }
//        public string Month4ButtonLabel { get; set; }


//        public Index()
//        {
//            ConnectionString = (new ConfigurationReader()).GetSetting("ConnectionStrings:MainConnection");

//            var options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<DataContext>(), ConnectionString).Options;

//            _dataContext = new DataContext(options);

//            var incomeExpenseSetups = GeneralHelper.ExecuteSQL<IncomeExpenseSetup>(_dataContext, "SELECT IncomeExpenseSetupId, StartDate, EndDate, IncomeExpenseType, Amount, Currency, Descr, NextPaymentDate, NextBillingDate, PaymentUrl, AccountLogInUrl FROM IncomeExpenseSetup");
//            IncomeExpenseSetups.AddRange(incomeExpenseSetups);

//            var currencyConversionRates = GeneralHelper.ExecuteSQL<CurrencyConversionRate>(_dataContext, "SELECT BaseCurrency, ToCurrency,BaseCurrencyId, ToCurrencyId, ConversionRate FROM [dbo].[CurrencyConversionRate]");
//            CurrencyConversionRates.AddRange(currencyConversionRates);

//            Month1ButtonLabel = DateTime.Now.ToString("MMM");
//            Month2ButtonLabel = FirstDayOfNextMonth(1).ToString("MMM");
//            Month3ButtonLabel = FirstDayOfNextMonth(2).ToString("MMM");
//            Month4ButtonLabel = FirstDayOfNextMonth(3).ToString("MMM");
//        }

//        public DateTime FirstDayOfNextMonth(int nextMonthAfter)
//        {
//            DateTime today = DateTime.Now;
//            if (nextMonthAfter > 1)
//            {
//                today = FirstDayOfNextMonth(nextMonthAfter - 1);
//            }

//            // Find the last day of the current month
//            DateTime lastDayOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));

//            // Find the first day of the next month
//            DateTime firstDayOfNextMonth = lastDayOfMonth.AddDays(1);
//            return firstDayOfNextMonth;

//        }

//        public List<string> GetConversionList(string baseCurrency, double amount)
//        {
//            var currenciesInUse = IncomeExpenseSetups.Select(x => x.Currency).Distinct().ToList();
//            var conversionList = new List<string>();
//            foreach (var currency in currenciesInUse)
//            {
//                var conversionRate = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrency == baseCurrency && x.ToCurrency == currency);
//                if (conversionRate != null)
//                {
//                    conversionList.Add($"{currency} {(int)(amount * (double)conversionRate.ConversionRate)}");
//                }
//                else
//                {
//                    var conversionRateReverse = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrency == currency && x.ToCurrency == baseCurrency);
//                    if (conversionRateReverse != null)
//                    {
//                        conversionList.Add($"{currency} {(int)(amount / (double)conversionRateReverse.ConversionRate)}");
//                    }
//                }
//            }
//            return conversionList;
//        }

      

//        private async Task btnExpenseSettings_Click(MouseEventArgs e, int monthIndex)
//        {

//        }
//    }
//}
