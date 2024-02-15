using BlazorBootstrap;
using System.Text.Json;
using ToyAssist.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ToyAssist.Web.Factories;
using ToyAssist.Web.DatabaseModels.Models;
using ToyAssist.Web.Mappers.ViewModelRepoMappers;
using ToyAssist.Web.Models;


namespace ToyAssist.Web.Pages
{

    public partial class PaymentHandlePopup
    {
        private ExpenseItemViewModel ExpenseItem { get; set; }
        private ExpensePaymentModel ExpensePayment { get; set; }
        private Modal ModalRef = default!;

        public PaymentHandlePopup()
        {
            ExpenseItem = new ExpenseItemViewModel();
        }

        [Parameter]
        public EventCallback<ExpensePaymentModel?> OnPaymentDataUpdatedEvent { get; set; }

        // This method will be called to raise the event.
        protected async virtual Task OnDataUpdatedEvent(ExpensePaymentModel? data)
        {
            await OnPaymentDataUpdatedEvent.InvokeAsync(data);
        }

        [Parameter]
        public required List<CurrencyModel> CurrenciesInUse { get; set; } = new List<CurrencyModel>();

        public bool IsShowCurrencyConversion { get; set; }

        public async Task ShowModalAsync(ExpenseItemViewModel? data, ExpensePaymentModel? expensePayment)
        {
            IsShowCurrencyConversion = true;
            ExpenseItem = data;
            ExpensePayment = expensePayment;
            await ModalRef.ShowAsync();
            StateHasChanged();
        }

        private async Task OnMarkAsPaidClick()
        {
            var dataContext = DataContextFactory.Create();

            // Update payment
            var expensePayment = dataContext.ExpensePayments.FirstOrDefault(x => x.ExpensePaymentId == ExpensePayment.ExpensePaymentId);
            if (expensePayment == null)
            {
                var expensePaymentToAdd = new ExpensePayment()
                {
                    AccountId = ExpensePayment.AccountId,
                    Amount = ExpenseItem.ExpenseSetup.Amount,
                    CreatedDateTime = DateTime.UtcNow,
                    ExpenseSetupId = ExpensePayment.ExpenseSetupId,
                    PaymentStatus = Enums.ExpensePaymentStatusEnum.Paid,
                    Month = ExpensePayment.Month,
                    Year = ExpensePayment.Year,
                    PaymentDoneDate = DateTime.UtcNow
                };

                await dataContext.ExpensePayments.AddAsync(expensePaymentToAdd);
                await dataContext.SaveChangesAsync();
                expensePayment = expensePaymentToAdd;
            }
            else if (expensePayment.PaymentStatus == Enums.ExpensePaymentStatusEnum.Pending)
            {
                expensePayment.PaymentDoneDate = DateTime.UtcNow;
                expensePayment.Amount = ExpenseItem.ExpenseSetup.Amount;
                expensePayment.PaymentStatus = Enums.ExpensePaymentStatusEnum.Paid;
                await dataContext.SaveChangesAsync();
            }
            // Refresh payment
            var expensePaymentModel = ExpensePaymentModelMapper.Map(expensePayment);
            await OnDataUpdatedEvent(expensePaymentModel);
            await OnHideModalClick();
        }

        private async Task OnMarkAsNotPaidClick()
        {
            var dataContext = DataContextFactory.Create();

            // Update payment
            var expensePayment = dataContext.ExpensePayments.FirstOrDefault(x => 
                                    x.ExpensePaymentId == ExpensePayment.ExpensePaymentId
                                    && x.ExpenseSetupId == ExpenseItem.ExpenseSetupId 
                                    && x.Year == ExpensePayment.Year
                                    && x.Month == ExpensePayment.Month);
            if (expensePayment != null)
            {
                expensePayment.PaymentDoneDate = null;
                expensePayment.PaymentStatus = Enums.ExpensePaymentStatusEnum.Pending;
                await dataContext.SaveChangesAsync();
            }
            // Refresh payment list
            var expensePaymentModel = ExpensePaymentModelMapper.Map(expensePayment);
            await OnDataUpdatedEvent(expensePaymentModel);
            await OnHideModalClick();
        }

        private async Task OnHideModalClick()
        {
            await ModalRef.HideAsync();
        }
    }
}
