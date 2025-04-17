using InvoiceBillingSystem.Data;
using InvoiceBillingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace InvoiceBillingSystem.Repositories
{
    public class CreditNoteRepository:ICreditNoteRepository
    {
        private readonly ApplicationDbContext _context;

        public CreditNoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateCreditNoteAsync(CreditNote creditNote)
        {
            _context.CreditNotes.Add(creditNote);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CreditNote>> GetCreditNotesByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.CreditNotes
                .Where(cn => cn.InvoiceId == invoiceId)
                .ToListAsync();
        }
    }
}
