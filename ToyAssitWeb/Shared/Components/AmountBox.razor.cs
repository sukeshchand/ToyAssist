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

    public partial class AmountBox
    {
        [Parameter]
        public required decimal? Amount { get; set; }

        [Parameter]
        public required decimal? TaxAmount { get; set; }

        [Parameter]
        public required Currency? Currency { get; set; }

    }
}
