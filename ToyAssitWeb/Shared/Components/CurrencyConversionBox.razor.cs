using System;
using System.Globalization;
using System.Text;

using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Factories;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Shared.Components
{

    public partial class CurrencyConversionBox
    {
        [Parameter]
        public required ExpenseSetupModel ModalData { get; set; }

        [Parameter]
        public required CurrencyModel CurrencyFrom { get; set; }

        [Parameter]
        public required List<CurrencyModel> CurrenciesTo { get; set; }

        [Parameter]
        public required decimal? Amount { get; set; }

        [Parameter]
        public bool IsShowBaseAmount { get; set; } = true;

        [Parameter]
        public bool IsShowBox { get; set; } = true;

        public string BorderStyle => IsShowBox ? "border: 1px solid #ccc; border-radius:10px;" : "";

    }
}
