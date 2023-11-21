using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class CurrencyConversionRate
{
    public int BaseCurrencyId { get; set; }

    public int ToCurrencyId { get; set; }

    public decimal ConversionRate { get; set; }

    public virtual Currency BaseCurrency { get; set; } = null!;

    public virtual Currency ToCurrency { get; set; } = null!;
}
