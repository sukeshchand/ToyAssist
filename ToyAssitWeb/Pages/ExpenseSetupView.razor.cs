using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ToyAssist.Web.DatabaseModels;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.Helpers;
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
            if (IsPostBack == false)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            var dataContext = DataContextFactory.Create();
            ExpenseSetups = dataContext.ExpenseSetups.ToList();
            CurrencyConversionRates = dataContext.CurrencyConversionRates.ToList();
        }

        protected override void OnInitialized()
        {
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

        //public List<string> GetConversionList(int baseCurrencyId, double amount)
        //{
        //    var currenciesInUse = ExpenseSetups.Select(x => x.CurrencyId).Distinct().ToList();
        //    var conversionList = new List<string>();
        //    foreach (var currencyId in currenciesInUse)
        //    {
        //        var conversionRate = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == baseCurrencyId && x.ToCurrencyId == currencyId);
        //        if (conversionRate != null)
        //        {
        //            conversionList.Add($"{currencyId} {(int)(amount * (double)conversionRate.ConversionRate)}");
        //        }
        //        else
        //        {
        //            var conversionRateReverse = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == currencyId && x.ToCurrencyId == baseCurrencyId);
        //            if (conversionRateReverse != null)
        //            {
        //                conversionList.Add($"{currencyId} {(int)(amount / (double)conversionRateReverse.ConversionRate)}");
        //            }
        //        }
        //    }
        //    return conversionList;
        //}

    }
}
