using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class ExpenseSetup
{
    public int ExpenseSetupId { get; set; }

    public int AccountId { get; set; }

    public string ExpenseName { get; set; } = null!;

    public string? ExpenseDescr { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Amount { get; set; }

    public decimal? TaxAmount { get; set; }

    public int? CurrencyId { get; set; }

    public int? BillGeneratedDay { get; set; }

    public int? BillPaymentDay { get; set; }

    public int? ExpirationDay { get; set; }

    public string? PaymentUrl { get; set; }

    public string? AccountProfileUrl { get; set; }

    /// <summary>
    /// Prepaid = 0, Postpaid = 1
    /// </summary>
    public bool? PrepaidOrPostpaid { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Currency? Currency { get; set; }
}
