using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using ToyAssist.Web.Enums;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class _DataContext : DbContext
{
    public _DataContext()
    {
    }

    public _DataContext(DbContextOptions<_DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyConversionRate> CurrencyConversionRates { get; set; }

    public virtual DbSet<ExpensePayment> ExpensePayments { get; set; }

    public virtual DbSet<ExpenseSetup> ExpenseSetups { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AcountId);

            entity.ToTable("Account");

            entity.Property(e => e.AcountId).ValueGeneratedNever();
            entity.Property(e => e.AccountName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.CurrencyId).HasName("PK_CurrencyMaster");

            entity.ToTable("Currency");

            entity.Property(e => e.CurrencyId).ValueGeneratedNever();
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(50);
        });

        modelBuilder.Entity<CurrencyConversionRate>(entity =>
        {
            entity.HasKey(e => new { e.BaseCurrencyId, e.ToCurrencyId });

            entity.ToTable("CurrencyConversionRate");

            entity.Property(e => e.ConversionRate).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BaseCurrency).WithMany(p => p.CurrencyConversionRateBaseCurrencies)
                .HasForeignKey(d => d.BaseCurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ToCurrency).WithMany(p => p.CurrencyConversionRateToCurrencies)
                .HasForeignKey(d => d.ToCurrencyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ExpensePayment>(entity =>
        {
            entity.ToTable("ExpensePayment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentDoneDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus).HasConversion(new EnumToNumberConverter<ExpensePaymentStatusEnum, int>());
        });

        modelBuilder.Entity<ExpenseSetup>(entity =>
        {
            entity.ToTable("ExpenseSetup");

            entity.Property(e => e.AccountProfileUrl).HasMaxLength(500);
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 4)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ExpenseDescr).HasMaxLength(500);
            entity.Property(e => e.ExpenseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentUrl).HasMaxLength(500);
            entity.Property(e => e.PrepaidOrPostpaid).HasComment("Prepaid = 0, Postpaid = 1");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(10, 4)");

            entity.HasOne(d => d.Account).WithMany(p => p.ExpenseSetups)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Currency).WithMany(p => p.ExpenseSetups).HasForeignKey(d => d.CurrencyId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
