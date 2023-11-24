﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<ExpenseSetup> ExpenseSetups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:toyassist.database.windows.net,1433;Initial Catalog=ToyAssist.Test;Persist Security Info=False;User ID=sukeshchand;Password=bNEcea5tJe@Nj5!r;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

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

            entity.HasOne(d => d.Account).WithMany(p => p.ExpenseSetups)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Currency).WithMany(p => p.ExpenseSetups).HasForeignKey(d => d.CurrencyId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}