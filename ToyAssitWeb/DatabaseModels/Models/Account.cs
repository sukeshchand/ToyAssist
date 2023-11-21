using System;
using System.Collections.Generic;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class Account
{
    public int AcountId { get; set; }

    public string AccountName { get; set; } = null!;
}
