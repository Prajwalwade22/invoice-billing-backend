using System;
using System.IO;
using InvoiceBillingSystem.Models;
using InvoiceBillingSystem.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace InvoiceBillingSystem.Services
{
    public class InvoicePdfService : IInvoicePdfService
    {
        private readonly IInvoicePdfRepository _invoicePdfRepository;
        private readonly string _logoPath;
        private readonly string _successIcon;
        private readonly string _failedIcon;


        public InvoicePdfService(IInvoicePdfRepository invoicePdfRepository)
        {
            _invoicePdfRepository = invoicePdfRepository;

            string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            _logoPath = Path.Combine(wwwRootPath, "invoicelogo.png");
            _successIcon = Path.Combine(wwwRootPath, "suuceesssfull.png");
            _failedIcon = Path.Combine(wwwRootPath, "pending.png");
        }

        public byte[] GenerateInvoicePdf(Guid invoiceId)
        {
            var invoice = _invoicePdfRepository.GetInvoiceById(invoiceId);
            if (invoice == null)
                throw new Exception("Invoice not found");

            using (var stream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(40);
                        page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Helvetica"));

                        page.Content().Column(col =>
                        {
                            col.Item().Row(row =>
                            {
                                if (File.Exists(_logoPath))
                                {
                                    row.RelativeItem().Column(imgCol =>
                                    {
                                        imgCol.Item().Height(60).Image(_logoPath, ImageScaling.FitArea);
                                    });
                                }

                                row.RelativeItem().AlignRight().Text("InvoiceBillingSystem")
                                    .FontSize(28).Bold().FontColor(Colors.Blue.Darken2);
                            });


                            col.Item().AlignCenter().Text("Your Reliable Billing Partner")
                                .FontSize(14).Italic().FontColor(Colors.Grey.Darken2);

                            col.Item().PaddingBottom(10);

                            var statusIconBytes = LoadImage(invoice.Status == "Paid" ? _successIcon : _failedIcon);
                            if (statusIconBytes != null)
                            {
                                col.Item().AlignCenter().Column(imgCol =>
                                {
                                    imgCol.Item().Height(50).Image(statusIconBytes);
                                });
                            }




                            col.Item().AlignCenter().Text(invoice.Status)
                                .FontSize(18).Bold().FontColor(invoice.Status == "Paid" ? Colors.Green.Darken2 : Colors.Red.Darken2);

                            col.Item().PaddingBottom(10);

                            col.Item().Border(1).BorderColor(Colors.Grey.Darken3).Padding(15).Column(invoiceCol =>
                            {
                                invoiceCol.Item().AlignCenter().Text("Invoice Details")
                                    .FontSize(20).Bold().FontColor(Colors.Black);

                                invoiceCol.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(180); 
                                        columns.RelativeColumn(); 
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().BorderBottom(1).Padding(5).Background(Colors.Blue.Lighten3)
                                            .Text("Field").FontSize(14).Bold();
                                        header.Cell().BorderBottom(1).Padding(5).Background(Colors.Blue.Lighten3)
                                            .Text("Value").FontSize(14).Bold();
                                    });

                                    AddTableRow(table, "Invoice ID", invoice.Id.ToString(), Colors.Grey.Darken3);
                                    AddTableRow(table, "User ID", invoice.UserId.ToString(), Colors.Grey.Darken2);
                                    AddTableRow(table, "Amount", $"${invoice.Amount:F2}", Colors.Grey.Darken3);
                                    AddTableRow(table, "Status", invoice.Status, Colors.Grey.Darken2);
                                    AddTableRow(table, "Due Date", invoice.DueDate.ToString("yyyy-MM-dd"), Colors.Grey.Darken3);
                                    AddTableRow(table, "Created At", invoice.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), Colors.Grey.Darken2);
                                });
                            });

                            col.Item().PaddingTop(15);

                            // Footer with Contact Info
                            col.Item().AlignCenter().Text("InvoiceBillingSystem | Contact: support@invoicebilling.com | www.invoicebilling.com")
                                .FontSize(10).FontColor(Colors.Grey.Darken2);
                        });
                    });
                }).GeneratePdf(stream);

                return stream.ToArray();
            }
        }

        // Helper function to add rows with alternating background colors
        private void AddTableRow(TableDescriptor table, string field, string value, string backgroundColor)
        {
            table.Cell().BorderBottom(1).Padding(5).Background(backgroundColor)
                .Text(field).FontSize(12).Bold().FontColor(Colors.White);

            table.Cell().BorderBottom(1).Padding(5)
                .Text(value).FontSize(12).FontColor(Colors.Black);
        }
        private byte[] LoadImage(string imagePath)
        {
            if (File.Exists(imagePath))
                return File.ReadAllBytes(imagePath);

            return null; 
        }

    }
}
