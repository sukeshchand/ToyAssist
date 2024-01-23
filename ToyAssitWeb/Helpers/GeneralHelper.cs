using LazyCache;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.TypeExtensions;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Helpers
{
    public class GeneralHelper
    {

        public static string GetOrdinalSuffix(int? number)
        {
            if (number == null) return string.Empty;
            if (number % 100 >= 11 && number % 100 <= 13)
                return "th";

            switch (number % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string GetMonthAndDaysLeftString(DateTime? date)
        {
            if (date == null) return string.Empty;

            var diff = (date - DateTime.Now).Value;
            if (diff.TotalDays > 30)
            {
                var months = (int)(diff.TotalDays / 30);
                var days = (int)(diff.TotalDays - (months * 30));
                return $"{months} months {days} days left";
            }
            return $"{(int)diff.TotalDays} days left";
        }

        public static int CalculateMonthDifference(DateTime startDate, DateTime endDate)
        {
            return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month;
        }

        public static (int Years, int Months, bool IsError) CalculateYearMonthDifference(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null) return (0, 0, true);

            var totalMonths = CalculateMonthDifference((DateTime)startDate, (DateTime)endDate);
            var yearsLeft = totalMonths / 12;
            var yearsAndMonthsLeft = totalMonths % 12;
            return (yearsLeft, yearsAndMonthsLeft, false);
        }

        public static List<(string Name, PropertyInfo PropertyInfo)> GetProperties(Object obj)
        {
            var properties = new List<(string, PropertyInfo)>();
            // Get the type of the object
            Type type = obj.GetType();

            // Get all public properties
            PropertyInfo[] publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Console.WriteLine("\nPublic Properties:");
            foreach (var property in publicProperties)
            {
                properties.Add(new(property.Name, property));
            }
            return properties;
        }

        public string GenerateHtmlTable<T>(List<T> dataList, List<int> columnWidths)
        {
            try
            {
                if (dataList == null || dataList.Count == 0)
                {
                    return "<p>No data to display.</p>";
                }

                var properties = GetProperties(dataList[0]);

                var tableHtml = new StringBuilder();

                tableHtml.Append("<div class='table-responsive'><table class='table table-bordered'><thead><tr>");
                int i = 0;
                foreach (var property in properties)
                {
                    tableHtml.Append($"<th style='width: {columnWidths[i]}px;'>{property.Name}</th>");
                    i++;
                }

                tableHtml.Append("</tr></thead><tbody>");
                foreach (var item in dataList)
                {
                    tableHtml.Append("<tr>");
                    i = 0;
                    foreach (var property in properties)
                    {
                        tableHtml.Append($"<td><div style='max-width: {columnWidths[i]}px; overflow: auto; max-height: 100px;' >{property.PropertyInfo.GetValue(item)}</div></td>");
                        i++;
                    }
                    tableHtml.Append("</tr>");
                }

                tableHtml.Append("</tbody></table></div>");

                return tableHtml.ToString();
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }

        public static int GetCurrentWeekNumber(DateTime? dateTime = null)
        {
            if (dateTime == null) dateTime = DateTime.Now;

            var currentCulture = CultureInfo.CurrentCulture;
            var weekNo = currentCulture.Calendar.GetWeekOfYear((DateTime)dateTime,
                            currentCulture.DateTimeFormat.CalendarWeekRule,
                            currentCulture.DateTimeFormat.FirstDayOfWeek);
            return weekNo;
        }

        public static List<T> ExecuteSQL<T>(_DataContext dataContext, string sql)
        {
            FormattableString sqlExec = FormattableStringFactory.Create(sql);
            return dataContext.Database.SqlQuery<T>(sqlExec).ToList();
        }

        public static string? FormattedAmount(decimal? amount, CurrencyViewModel? currency = null)
        {
            if (amount == null) { return null; }

            var str = amount.ToStringCustom();
            if (currency != null)
            {
                str = $"{str} {currency.CurrencyCode}";
            }
            return str;
        }

        public static List<CurrencyConversionRate> CurrencyConversionRates()
        {
            var cache = new CachingService();
            var cachedResults = cache.GetOrAdd("__CurrencyConversionRates", CurrencyConversionRatesNotCached, new TimeSpan(0, 0, 60, 0));
            return cachedResults;
        }

        public static List<CurrencyConversionRate> CurrencyConversionRatesNotCached()
        {
            var dataContext = DataContextFactory.Create();
            var currencyConversionRates = dataContext.CurrencyConversionRates.ToList();
            return currencyConversionRates;
        }

        public static List<string> GetConversionList(CurrencyViewModel fromCurrency, List<CurrencyViewModel> toCurrencies, decimal amount)
        {
            var currencyConversionRates = GeneralHelper.CurrencyConversionRates();
            var toCurrenciesActual = toCurrencies.Where(c => c.CurrencyId != fromCurrency.CurrencyId);
            var conversionList = new List<string>();
            decimal amountConverted = 0;
            foreach (var currency in toCurrenciesActual)
            {
                var conversionRate = currencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == fromCurrency.CurrencyId && x.ToCurrencyId == currency!.CurrencyId);
                amountConverted = (amount * conversionRate?.ConversionRate ?? 0);
                if (conversionRate != null)
                {
                    conversionList.Add($"{amountConverted.ToStringCustom()} {currency.CurrencyCode}");
                }
                else
                {
                    var conversionRateReverse = currencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == currency.CurrencyId && x.ToCurrencyId == fromCurrency.CurrencyId);
                    if (conversionRateReverse != null)
                    {
                        amountConverted = (amount / conversionRateReverse.ConversionRate);
                        conversionList.Add($"{amountConverted.ToStringCustom()} {currency.CurrencyCode}");
                    }
                }
            }
            return conversionList;
        }

        public static List<ExpenseRunningModel> GetExpenseRunningList(ExpenseSetupViewModel expenseSetup)
        {
            var list = new List<ExpenseRunningModel>();

            DateTime? startDate = null;
            DateTime endDate = expenseSetup.EndDate != null ? (DateTime)expenseSetup.EndDate : DateTime.Now;

            if (expenseSetup.StartDate != null)
            {
                startDate = (DateTime)expenseSetup.StartDate;
            }


            if (startDate == null) return list;
            var firstPaymentDate = GetFirstBillPaymentDate(expenseSetup, (DateTime)startDate);
            var currentDate = firstPaymentDate;
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
            // End of method - return list

            // Local methods
            DateTime GetFirstBillPaymentDate(ExpenseSetupViewModel expenseSetup, DateTime startDate)
            {
                if (expenseSetup.BillPaymentDay == null)
                {
                    return startDate;
                }
                try
                {
                    return new DateTime(startDate.Year, startDate.Month, (int)expenseSetup.BillPaymentDay);
                }
                catch (Exception ex)
                {
                    // handle invalid date due to 31st or 30th feb etc in some months
                    return startDate;
                }
            }
        }

        public static string GetMonthName(int monthNumber, string format = "MMMM")
        {
            var dt = new DateTime(2020, monthNumber, 13);
            return dt.ToString(format);
        }
    }
}
