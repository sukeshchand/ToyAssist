using Microsoft.AspNetCore.Components;
using ToyAssist.Web.ViewModels;

namespace ToyAssist.Web.Pages
{

    public partial class ExpenseSetupViewTotals
    {
        [Parameter]
        public required ExpenseSetupViewModel ModalData { get; set; }
    }
}
