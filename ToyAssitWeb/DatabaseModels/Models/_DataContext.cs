using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ToyAssist.Web.DatabaseModels.Models;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<CurrencyConversionRate> CurrencyConversionRates { get; set; }

    public virtual DbSet<ExpenseSetup> ExpenseSetups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

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
            entity.Property(e => e.CurrencySymbol).HasMaxLength(50);
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CurrencyConversionRate>(entity =>
        {
            entity.HasKey(e => new { e.BaseCurrency, e.ToCurrency }).HasName("PK_CurrencyRate");

            entity.ToTable("CurrencyConversionRate");

            entity.Property(e => e.BaseCurrency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ToCurrency)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ConversionRate).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<ExpenseSetup>(entity =>
        {
            entity.ToTable("ExpenseSetup");

            entity.Property(e => e.AccountProfileUrl).HasMaxLength(500);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ExpenseDescr).HasMaxLength(500);
            entity.Property(e => e.ExpenseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentUrl).HasMaxLength(500);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
