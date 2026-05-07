using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;
using PruebaTecnicaFacundoTobioBack.Infrastructure.Data;

namespace PruebaTecnicaFacundoTobioBack.Infrastructure.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesWithItemsAsync()
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .OrderByDescending(i => i.Fecha)
                .ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceWithItemsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);
        }
    }
}
