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

        private List<ExpenseRunningModel> GetExpenseRunningList(ExpenseSetupViewModel expenseSetup)
        {
            var list = new List<ExpenseRunningModel>();

            DateTime? startDate = null;
            DateTime endDate = @ModalData.EndDate != null ? (DateTime)@ModalData.EndDate : DateTime.Now;

            if (@ModalData.StartDate != null)
            {
                startDate = (DateTime)@ModalData.StartDate;
            }


            if (startDate == null) return list;

            
            var currentDate =(DateTime)startDate;
            var index = 0;
            var currentItemAssigned = false;

            do
            {
                var listItem = new ExpenseRunningModel
                {
                    Index = index + 1,
                    DateAndTime = currentDate,
                    Amount = expenseSetup?.Amount ?? 0,
                    Status = currentDate > DateTime.Now ? "Pending" : "Paid",
                    TotalAmount = (expenseSetup?.Amount ?? 0) + (expenseSetup?.TaxAmount ?? 0)
                };
                if ((index + 1) % 12 == 0)
                {
                    listItem.IsYearBreak = true;
                }

                if ((!currentItemAssigned && index > 0 && list[index - 1].Status == "Paid" && listItem.Status == "Pending") ||
                        (listItem.Status == "Pending" && index == 0))
                {
                    listItem.IsCurrentItem = true;
                    currentItemAssigned = true;
                }

                // ------------------------ Calculations end ------------------------
                list.Add(listItem);
                currentDate = currentDate.AddMonths(1);
                index++;

            } while (endDate > currentDate);
            return list;
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
