using InvoiceBillingSystem.Repositories;
using InvoiceBillingSystem.Services;

namespace InvoiceBillingSystem.BackGroundServices
{
    public class InvoiceReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public InvoiceReminderService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var overdueInvoices = await invoiceRepository.GetOverdueInvoicesAsync();
                    foreach (var invoice in overdueInvoices)
                    {
                        await notificationService.SendOverdueReminderAsync(invoice);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}



//using InvoiceBillingSystem.Repositories;
//using InvoiceBillingSystem.Services;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace InvoiceBillingSystem.BackGroundServices
//{
//    public class InvoiceReminderService : BackgroundService
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;

//        public InvoiceReminderService(IServiceScopeFactory serviceScopeFactory)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())
//                {
//                    var invoiceRepository = scope.ServiceProvider.GetRequiredService<IInvoiceRepository>();
//                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

//                    var overdueInvoices = await invoiceRepository.GetOverdueInvoicesAsync();
//                    foreach (var invoice in overdueInvoices)
//                    {
//                        await notificationService.SendOverdueReminderAsync(invoice);
//                    }

//                    var unpaidOverdueInvoices = await invoiceRepository.GetOverdueInvoicesAsync();
//                    foreach (var invoice in unpaidOverdueInvoices)
//                    {
//                        decimal interestAmount = invoiceRepository.CalculateOverdueInterest(invoice);

//                        if (interestAmount > 0)
//                        {
//                            await invoiceRepository.ApplyOverdueInterestAsync(invoice.Id, interestAmount);
//                        }
//                    }
//                }

//                await Task.Delay(TimeSpan.FromSeconds(120), stoppingToken);
//            }
//        }
//    }
//}
