using System.ComponentModel.Design;
using Google.Apis.Drive.v3.Data;
using InvoiceBillingSystem.DTO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;

namespace InvoiceBillingSystem.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly INotificationService _notificationService;
        private readonly IAuditLogService _auditLogService;
        private readonly IDiscountService _discountService;
        private readonly CurrencyConverterService _currencyConverterService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _Configuration;


        public InvoiceService(IInvoiceRepository invoiceRepository,INotificationService notificationService,IAuditLogService auditLogService,IDiscountService discountService,CurrencyConverterService currencyConverterService,IUserRepository userRepository,IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _notificationService = notificationService;
            _auditLogService = auditLogService;
            _discountService = discountService;
            _currencyConverterService = currencyConverterService;
            _userRepository = userRepository;
            _Configuration = configuration;
        }

        public async Task<InvoiceDto> CreateInvoiceAsync(CreateInvoiceDto invoiceDto)
        { 
            var user=await _userRepository.GetUserByIdAsync(invoiceDto.UserId);
            if (user == null)
            {
                throw new Exception("User Id not Found.Please Register User First.");
            }

            string baseCurrency = _Configuration["CurrencySettings:BaseCurrency"]; 

            decimal convertedAmount = await _currencyConverterService.ConvertCurrencyAsync(
                invoiceDto.Amount, baseCurrency, invoiceDto.Currency);

            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                UserId = invoiceDto.UserId,
                Amount = invoiceDto.Amount,
                Currency=invoiceDto.Currency,
                AmountInSelectedCurrency = convertedAmount,
                Status = "Pending",
                DueDate = invoiceDto.DueDate.AddDays(2),
                //DueDate=invoiceDto.DueDate.AddSeconds(120),
                CreatedAt = DateTime.UtcNow,
                CompanyId= invoiceDto.CompanyId,
                IsSigned=true
            };

            invoice.DiscountApplied = await _discountService.ApplyDynamicDiscountAsync(invoice);

            await _invoiceRepository.CreateInvoiceAsync(invoice);

            invoice.User = await _invoiceRepository.GetUserByIdAsync(invoice.UserId);

            if (invoice.User == null)
            {
                throw new Exception("User not found for this invoice.");
            }

            await _notificationService.SendInvoiceNotificationAsync(invoice);

            return new InvoiceDto
            {
                Id = invoice.Id,
                UserId = invoice.UserId,
                Amount = invoice.Amount,
                Status = invoice.Status,
                Currency=invoice.Currency,
                AmountInSelectedCurrency = invoice.AmountInSelectedCurrency,
                DueDate = invoice.DueDate,
                CreatedAt = invoice.CreatedAt,
                CompanyId=invoice.CompanyId
            };
        }

        public async Task<InvoiceDto?> GetInvoiceByIdAsync(Guid invoiceId)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);
            return invoice == null ? null : new InvoiceDto
            {
                Id = invoice.Id,
                UserId = invoice.UserId,
                Amount = invoice.Amount,
                Status = invoice.Status,
                DueDate=invoice.DueDate,
                CreatedAt = invoice.CreatedAt,
                Currency=invoice.Currency,
                AmountInSelectedCurrency=invoice.AmountInSelectedCurrency,
                CompanyId=invoice.CompanyId
            };
        }

        public async Task<IEnumerable<InvoiceDto>> GetInvoicesByUserIdAsync(Guid userId)
        {
            var invoices = await _invoiceRepository.GetInvoicesByUserIdAsync(userId);
            return invoices.Select(invoice => new InvoiceDto
            {
                Id = invoice.Id,
                UserId = invoice.UserId,
                Amount = invoice.Amount,
                Status = invoice.Status,
                CreatedAt = invoice.CreatedAt,
                DueDate=invoice.DueDate,
                Currency = invoice.Currency,
                AmountInSelectedCurrency = invoice.AmountInSelectedCurrency,
                CompanyId = invoice.CompanyId
            });
        }

        public async Task<bool> UpdateInvoiceStatusAsync(Guid invoiceId, string status)
        {
            var invoice = await _invoiceRepository.GetInvoiceByIdAsync(invoiceId);
            if (invoice == null) return false;

            invoice.Status = status;
            await _invoiceRepository.UpdateInvoiceStatusAsync(invoiceId, status);

            await _auditLogService.LogActionAsync("Invoice Updated", "System",
                $"Invoice {invoiceId} status changed to {status}");

            return true;
        }

        public async Task ApplyOverdueInterestAsync()
        {
            var overdueInvoices = await _invoiceRepository.GetOverdueInvoicesAsync();

            foreach (var invoice in overdueInvoices)
            {
                int overdueDays = (DateTime.UtcNow - invoice.DueDate).Days;
                if (overdueDays > 0)
                {
                    decimal dailyInterestRate = 0.05m / 100; 
                    decimal interestAmount = invoice.Amount * dailyInterestRate * overdueDays;

                    invoice.OverdueInterest = interestAmount;
                    invoice.TotalAmountDue = invoice.Amount + invoice.OverdueInterest;

                    await _invoiceRepository.UpdateInvoiceAsync(invoice);

                    await _notificationService.SendOverdueReminderAsync(invoice);
                }
            }
        }


    }
}
