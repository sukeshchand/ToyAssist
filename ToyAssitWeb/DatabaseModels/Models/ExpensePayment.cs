using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class ExpensePayment
{
    public long ExpensePaymentId { get; set; }

    public int ExpenseSetupId { get; set; }

    public int AccountId { get; set; }

    public int? Year { get; set; }

    public int? Month { get; set; }

    public DateTime? PaymentDoneDate { get; set; }

    public int ExpensePaymentStatus { get; set; }

    public string? Descr { get; set; }

    public DateTime CreatedDateTime { get; set; }
}
