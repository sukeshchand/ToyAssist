using BlazorBootstrap;
using System.Text.Json;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Identity.Client;
using ToyAssist.Web.Factories;


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
            var dataContext = DataContextFactory.Create();

            var expensePayment = dataContext.ExpensePayments.FirstOrDefault(x => x.ExpenseSetupId == ModalData.ExpenseSetupId && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
            if (expensePayment == null)
            {
                var expensePaymentToAdd = new ExpensePayment()
                {
                    AccountId = ModalData.AccountId,
                    CreatedDateTime = DateTime.Now,
                    ExpenseSetupId = ModalData.ExpenseSetupId,
                    ExpensePaymentStatus = Enums.ExpensePaymentStatusEnum.Done,
                    Month = DateTime.Now.Month,
                    Year = DateTime.Now.Year,
                    PaymentDoneDate = DateTime.Now
                };

                await dataContext.ExpensePayments.AddAsync(expensePaymentToAdd);
                await dataContext.SaveChangesAsync();
            }
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
