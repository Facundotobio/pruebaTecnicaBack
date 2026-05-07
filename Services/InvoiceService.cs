using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Data;
using PruebaTecnicaFacundoTobioBack.DTOs;
using PruebaTecnicaFacundoTobioBack.Interfaces;
using PruebaTecnicaFacundoTobioBack.Models;

namespace PruebaTecnicaFacundoTobioBack.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Items)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();

            return invoices.Select(i => MapToResponseDto(i));
        }

        public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null) return null;

            return MapToResponseDto(invoice);
        }

        public async Task<InvoiceResponseDto> CreateAsync(InvoiceCreateDto invoiceDto)
        {
            var invoice = new Invoice
            {
                CustomerId = invoiceDto.CustomerId,
                Numero = invoiceDto.Numero,
                Fecha = DateTime.UtcNow,
                Items = invoiceDto.Items.Select(item => new InvoiceItem
                {
                    Descripcion = item.Descripcion,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario
                }).ToList()
            };

            // Calculate total
            invoice.Total = invoice.Items.Sum(item => item.Subtotal);

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return MapToResponseDto(invoice);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return false;

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        private static InvoiceResponseDto MapToResponseDto(Invoice invoice)
        {
            return new InvoiceResponseDto
            {
                InvoiceId = invoice.InvoiceId,
                CustomerId = invoice.CustomerId,
                Fecha = invoice.Fecha,
                Numero = invoice.Numero,
                Total = invoice.Total,
                Estado = invoice.Estado.ToString(),
                Items = invoice.Items.Select(item => new InvoiceItemResponseDto
                {
                    InvoiceItemId = item.InvoiceItemId,
                    Descripcion = item.Descripcion,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Subtotal = item.Subtotal
                }).ToList()
            };
        }
    }
}
