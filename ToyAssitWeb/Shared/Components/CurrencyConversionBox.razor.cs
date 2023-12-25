using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Shared.Components
{

    public partial class CurrencyConversionBox
    {
        [Parameter]
        public required ExpenseSetup ModalData { get; set; }

        [Parameter]
        public required Currency CurrencyFrom { get; set; }

        [Parameter]
        public required List<Currency> CurrenciesTo { get; set; }

        [Parameter]
        public required decimal Amount { get; set; }

    }
}
