using System;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBillingSystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User>Users { get; set; }
        public DbSet<Invoice>Invoices { get; set; } 

        public DbSet<Payment>Payments { get; set; } 

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Company>Companies { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<PaymentLink> PaymentLinks { get; set; }

        public DbSet<OTP>OTP { get; set; }

        public DbSet<UserActivity> UserActivities { get; set; }

        public DbSet<CreditNote> CreditNotes { get; set; }

        public DbSet<InvestmentTransaction> InvestmentTransactions { get; set; }

        public DbSet<LoanApplication> LoanApplications { get; set; }

        public DbSet<SmsRequest> SmsRequest { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        //public DbSet<ScheduledSms>ScheduledSms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Company)
                .HasForeignKey(u => u.CompanyId);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Company)
                .WithMany()
                .HasForeignKey(i => i.CompanyId);
        }
    }
}
