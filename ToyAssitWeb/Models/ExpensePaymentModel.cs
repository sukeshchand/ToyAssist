﻿using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Enums;

namespace ToyAssist.Web.Models
{
    public class ExpensePaymentModel
    {
        public long ExpensePaymentId { get; set; }
        public int Index { get; set; }

        public int ExpenseSetupId { get; set; }

        public int AccountId { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }

        public DateTime? PaymentDoneDate { get; set; }

        public ExpensePaymentStatusEnum? PaymentStatus { get; set; }

        public string? Descr { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
