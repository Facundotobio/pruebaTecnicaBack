using PruebaTecnicaFacundoTobioBack.Application.DTOs;

namespace PruebaTecnicaFacundoTobioBack.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceResponseDto>> GetAllAsync();
        Task<InvoiceResponseDto?> GetByIdAsync(int id);
        Task<InvoiceResponseDto> CreateAsync(InvoiceCreateDto invoiceDto);
        Task<bool> DeleteAsync(int id);
    }
}
