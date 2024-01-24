using Microsoft.AspNetCore.Components;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetupViewTotals
    {
        [Parameter]
        public required ExpenseSetupModel ModalData { get; set; }
    }
}
