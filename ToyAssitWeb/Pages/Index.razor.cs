using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ToyAssist.Web.DatabaseModels;
using ToyAssist.Web.Helpers;

namespace ToyAssist.Web.Pages
{
    public partial class Index
    {
        public string ConnectionString;
        private List<string> results = new List<string>();

        public IConfiguration _configuration { get; }

        List<IncomeExpenseSetup> IncomeExpenseSetups = new List<IncomeExpenseSetup>();
        private readonly DataContext _dataContext;

        public string Month1ButtonLabel { get; set; }
        public string Month2ButtonLabel { get; set; }
        public string Month3ButtonLabel { get; set; }
        public string Month4ButtonLabel { get; set; }


        public Index()
        {
            ConnectionString = (new ConfigurationReader()).GetSetting("ConnectionStrings:MainConnection");

            var options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<DataContext>(), ConnectionString).Options;

            _dataContext = new DataContext(options);

            var incomeExpenseSetups = ExecuteSQL<IncomeExpenseSetup>("SELECT IncomeExpenseSetupId, StartDate, EndDate, IncomeExpenseType, Amount, Currency, Descr, NextPaymentDate, NextBillingDate, PaymentUrl, AccountLogInUrl FROM IncomeExpenseSetup");
            IncomeExpenseSetups.AddRange(incomeExpenseSetups);

            Month1ButtonLabel = DateTime.Now.ToString("MMM");
            Month2ButtonLabel = FirstDayOfNextMonth(1).ToString("MMM");
            Month3ButtonLabel = FirstDayOfNextMonth(2).ToString("MMM");
            Month4ButtonLabel = FirstDayOfNextMonth(3).ToString("MMM");
        }

        public DateTime FirstDayOfNextMonth(int nextMonthAfter)
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

        public string GetMonthAndDaysLeftString(DateTime? date)
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


        private List<T> ExecuteSQL<T>(string sql)
        {
            FormattableString sqlExec = FormattableStringFactory.Create(sql);
            return _dataContext.Database.SqlQuery<T>(sqlExec).ToList();
        }

        private async Task btnExpenseSettings_Click(MouseEventArgs e, int monthIndex)
        {

        }
    }
}
