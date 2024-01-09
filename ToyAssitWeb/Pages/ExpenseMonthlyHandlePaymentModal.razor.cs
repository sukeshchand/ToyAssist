using BlazorBootstrap;
using System.Text.Json;
using ToyAssist.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToyAssist.Web.Factories;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Mappers.ViewModelRepoMappers;


namespace ToyAssist.Web.Pages
{


    public partial class ExpenseMonthlyHandlePaymentModal
    {
        public ExpenseMonthlyHandlePaymentModal()
        {
            ModalData = new ExpenseItemViewModel();
        }

        [Parameter]
        public EventCallback<ExpenseItemViewModel?> OnPaymentDataUpdatedEvent { get; set; }

        // This method will be called to raise the event.
        protected async virtual Task OnDataUpdatedEvent(ExpenseItemViewModel? data)
        {
            await OnPaymentDataUpdatedEvent.InvokeAsync(data);
        }

        private ExpenseItemViewModel ModalData { get; set; }
        private Modal ModalRef = default!;

        [Parameter]
        public required List<CurrencyViewModel> CurrenciesInUse { get; set; } = new List<CurrencyViewModel>();

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

            // Update payment
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
            else if (expensePayment.ExpensePaymentStatus == Enums.ExpensePaymentStatusEnum.Pending)
            {
                expensePayment.PaymentDoneDate = DateTime.UtcNow;
                expensePayment.ExpensePaymentStatus = Enums.ExpensePaymentStatusEnum.Done;
                await dataContext.SaveChangesAsync();
            }
            // Refresh payment list
            var expensePayments = dataContext.ExpensePayments.Where(x => x.ExpenseSetupId == ModalData.ExpenseSetupId).ToList();
            ModalData.ExpensePayments = expensePayments.Select(x => ExpensePaymentViewModelMapper.Map(x, x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month)).ToList();
            await OnDataUpdatedEvent(ModalData);
            await OnHideModalClick();
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
