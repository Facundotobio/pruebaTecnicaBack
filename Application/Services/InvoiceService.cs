using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Application.Interfaces;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;

namespace PruebaTecnicaFacundoTobioBack.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAllAsync()
        {
            var invoices = await _invoiceRepository.GetInvoicesWithItemsAsync();
            return invoices.Select(i => MapToResponseDto(i));
        }

        public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);
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

            // El total se calcula en la entidad o aquí
            invoice.Total = invoice.Items.Sum(item => item.Subtotal);

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();

            return MapToResponseDto(invoice);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null) return false;

            _invoiceRepository.Remove(invoice);
            await _invoiceRepository.SaveChangesAsync();
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
