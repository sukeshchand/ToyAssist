using System.Text.Json;

using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToyAssist.Web.Models;

namespace ToyAssist.Web.Pages
{
    public partial class ExpenseOverviewPopup
    {
        public ExpenseOverviewPopup()
        {
            ModalData = new ExpenseSetupModel();
        }
        private ExpenseSetupModel ModalData { get; set; }
        private Modal ModalRef = default!;

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            WriteIndented = true, // Optional: Make the output more readable
        };

        private ElementReference elementRefToScrollInto = default;

        private async Task ScrollIntoDivAsync(ElementReference elementRefToScrollInto)
        {
            await JSRuntime.InvokeVoidAsync("scrollIntoView", elementRefToScrollInto);
        }

        public async Task ShowModalAsync(ExpenseSetupModel? data)
        {
            ModalData = data;
            await ModalRef.ShowAsync();
            await ScrollIntoDivAsync(elementRefToScrollInto);
            StateHasChanged();
        }
                

        private async Task OnShowModalClick(ExpenseSetupModel? data)
        {
            
        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
