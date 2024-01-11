using System.Text.Json;

using BlazorBootstrap;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToyAssist.Web.ViewModels;


namespace ToyAssist.Web.Pages
{
    public partial class ExpenseSetupViewModal
    {
        public ExpenseSetupViewModal()
        {
            ModalData = new ExpenseSetupViewModel();
        }
        private ExpenseSetupViewModel ModalData { get; set; }
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

        public async Task ShowModalAsync(ExpenseSetupViewModel? data)
        {
            ModalData = data;
            await ModalRef.ShowAsync();
            await ScrollIntoDivAsync(elementRefToScrollInto);
            StateHasChanged();
        }
                

        private async Task OnShowModalClick(ExpenseSetupViewModel? data)
        {
            
        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
