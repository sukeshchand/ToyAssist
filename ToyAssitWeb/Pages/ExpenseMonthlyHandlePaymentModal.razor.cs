using BlazorBootstrap;
using System.Text.Json;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace ToyAssist.Web.Pages
{
    public partial class ExpenseMonthlyHandlePaymentModal
    {
        public ExpenseMonthlyHandlePaymentModal()
        {
            ModalData = new ExpenseItemViewModel();
        }

        private ExpenseItemViewModel ModalData { get; set; }
        private Modal ModalRef = default!;

        [Parameter]
        public required List<Currency> CurrenciesInUse { get; set; } = new List<Currency>();

        public bool IsShowCurrencyConversion { get; set; }

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

        public async Task ShowModalAsync(ExpenseItemViewModel? data)
        {
            IsShowCurrencyConversion = true;
            ModalData = data;
            await ModalRef.ShowAsync();
            await ScrollIntoDivAsync(elementRefToScrollInto);
            StateHasChanged();
        }

       
        private async Task OnShowModalClick()
        {
            
        }

        private async Task OnMarkAsPaidClick()
        {

        }

        private async Task OnMarkAsNotPaidClick()
        {

        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
