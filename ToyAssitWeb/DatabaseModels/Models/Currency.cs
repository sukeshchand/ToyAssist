using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class Currency
{
    public int CurrencyId { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string CurrencyName { get; set; } = null!;

    public string? CurrencySymbol { get; set; }
}
