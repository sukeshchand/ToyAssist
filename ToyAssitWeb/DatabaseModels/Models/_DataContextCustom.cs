using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ToyAssist.Web.Enums;

namespace ToyAssist.Web.DatabaseModels.Models
{
    public partial class _DataContext : DbContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpensePayment>(entity =>
            {
                entity.Property(e => e.PaymentStatus)
                .HasDefaultValue(ExpensePaymentStatusEnum.Undefined)
                .HasConversion(new EnumToNumberConverter<ExpensePaymentStatusEnum, int>());
            });
        }
    }
}