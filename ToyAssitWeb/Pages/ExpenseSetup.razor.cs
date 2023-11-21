﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ToyAssist.Web.DatabaseModels;
using ToyAssist.Web.Helpers;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetup
    {
        public string ConnectionString;

        List<ExpenseSetupModel> ExpenseSetups = new List<ExpenseSetupModel>();
        List<CurrencyConversionRate> CurrencyConversionRates = new List<CurrencyConversionRate>();
        private readonly DataContext _dataContext;

    
        public ExpenseSetup()
        {
            ConnectionString = (new ConfigurationReader()).GetSetting("ConnectionStrings:MainConnection");

            var options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<DataContext>(), ConnectionString).Options;

            _dataContext = new DataContext(options);

            var expenseSql = @"SELECT EX.ExpenseSetupId, EX.UserId, EX.ExpenseName, EX.ExpenseDescr, EX.StartDate, EX.EndDate, EX.Amount, 
                                EX.CurrencyId, CR.CurrencyCode, EX.BillGeneratedDay, EX.BillPaymentDay, EX.ExpirationDay, EX.PaymentUrl, EX.AccountProfileUrl 
                                FROM dbo.ExpenseSetup EX Left Outer Join  [dbo].[Currency] CR ON EX.CurrencyId = CR.CurrencyId
                               "; 
            var expenseSetup = GeneralHelper.ExecuteSQL<ExpenseSetupModel>(_dataContext, expenseSql);
            ExpenseSetups.AddRange(expenseSetup);

            var currencyConversionRates = GeneralHelper.ExecuteSQL<CurrencyConversionRate>(_dataContext, "SELECT BaseCurrency, ToCurrency,BaseCurrencyId, ToCurrencyId, ConversionRate FROM [dbo].[CurrencyConversionRate]");
            CurrencyConversionRates.AddRange(currencyConversionRates);
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

        public List<string> GetConversionList(int baseCurrencyId, double amount)
        {
            var currenciesInUse = ExpenseSetups.Select(x => x.CurrencyId).Distinct().ToList();
            var conversionList = new List<string>();
            foreach (var currencyId in currenciesInUse)
            {
                var conversionRate = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == baseCurrencyId && x.ToCurrencyId == currencyId);
                if (conversionRate != null)
                {
                    conversionList.Add($"{currencyCode} {(int)(amount * (double)conversionRate.ConversionRate)}");
                }
                else
                {
                    var conversionRateReverse = CurrencyConversionRates.FirstOrDefault(x => x.BaseCurrencyId == currencyCode && x.ToCurrencyId == baseCurrencyId);
                    if (conversionRateReverse != null)
                    {
                        conversionList.Add($"{currencyCode} {(int)(amount / (double)conversionRateReverse.ConversionRate)}");
                    }
                }
            }
            return conversionList;
        }

    }
}
