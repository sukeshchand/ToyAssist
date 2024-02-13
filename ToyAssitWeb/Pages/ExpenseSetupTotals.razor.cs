using Microsoft.AspNetCore.Components;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetupTotals
    {
        [Parameter]
        public required ExpenseSetupModel ModalData { get; set; }
    }
}
