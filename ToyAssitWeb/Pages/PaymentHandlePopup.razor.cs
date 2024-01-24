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


    public partial class PaymentHandlePopup
    {
        public PaymentHandlePopup()
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
                    CreatedDateTime = DateTime.UtcNow,
                    ExpenseSetupId = ModalData.ExpenseSetupId,
                    PaymentStatus = Enums.ExpensePaymentStatusEnum.Paid,
                    Month = DateTime.Now.Month,
                    Year = DateTime.Now.Year,
                    PaymentDoneDate = DateTime.UtcNow
                };

                await dataContext.ExpensePayments.AddAsync(expensePaymentToAdd);
                await dataContext.SaveChangesAsync();
            }
            else if (expensePayment.PaymentStatus == Enums.ExpensePaymentStatusEnum.Pending)
            {
                expensePayment.PaymentDoneDate = DateTime.UtcNow;
                expensePayment.PaymentStatus = Enums.ExpensePaymentStatusEnum.Paid;
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
            var dataContext = DataContextFactory.Create();

            // Update payment
            var expensePayment = dataContext.ExpensePayments.FirstOrDefault(x => x.ExpenseSetupId == ModalData.ExpenseSetupId && x.Month == DateTime.Now.Month && x.Year == DateTime.Now.Year);
            if (expensePayment != null)
            {
                expensePayment.PaymentDoneDate = null;
                expensePayment.PaymentStatus = Enums.ExpensePaymentStatusEnum.Pending;
                await dataContext.SaveChangesAsync();
            }
            // Refresh payment list
            var expensePayments = dataContext.ExpensePayments.Where(x => x.ExpenseSetupId == ModalData.ExpenseSetupId).ToList();
            ModalData.ExpensePayments = expensePayments.Select(x => ExpensePaymentViewModelMapper.Map(x, x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month)).ToList();
            await OnDataUpdatedEvent(ModalData);
            await OnHideModalClick();
        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
