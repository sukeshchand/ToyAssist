using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class CurrencyConversionRate
{
    public string BaseCurrency { get; set; } = null!;

    public string ToCurrency { get; set; } = null!;

    public int? BaseCurrencyId { get; set; }

    public int? ToCurrencyId { get; set; }

    public decimal ConversionRate { get; set; }
}
