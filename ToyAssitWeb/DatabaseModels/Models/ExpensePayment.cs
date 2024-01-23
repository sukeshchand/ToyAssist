using System;
using System.Collections.Generic;
using ToyAssist.Web.Enums;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class ExpensePayment
{
    public long ExpensePaymentId { get; set; }

    public int ExpenseSetupId { get; set; }

    public int AccountId { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? Day { get; set; }

    public DateTime? PaymentDoneDate { get; set; }

    public ExpensePaymentStatusEnum ExpensePaymentStatus { get; set; }

    public string? Descr { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public decimal? Amount { get; set; }
}
