using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;
using PruebaTecnicaFacundoTobioBack.Infrastructure.Data;

namespace PruebaTecnicaFacundoTobioBack.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetCustomerWithInvoicesAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
