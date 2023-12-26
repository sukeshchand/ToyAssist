using BlazorBootstrap;
using System.Text.Json;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.CodeAnalysis;


namespace ToyAssist.Web.Pages
{
    public partial class ExpenseMontlyHandlePaymentModal
    {
        public ExpenseMontlyHandlePaymentModal()
        {
            ModalData = new ExpenseSetup();
        }
        private ExpenseSetup ModalData { get; set; }
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

        public async Task ShowModalAsync(ExpenseSetup? data)
        {
            ModalData = data;
            await ModalRef.ShowAsync();
            await ScrollIntoDivAsync(elementRefToScrollInto);
            StateHasChanged();
        }

       
        private async Task OnShowModalClick(ExpenseSetup? data)
        {
            
        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
