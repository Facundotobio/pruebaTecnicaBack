using PruebaTecnicaFacundoTobioBack.Domain.Entities;

namespace PruebaTecnicaFacundoTobioBack.Domain.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<IEnumerable<Invoice>> GetInvoicesWithItemsAsync();
        Task<Invoice?> GetInvoiceWithItemsAsync(int id);
    }
}
