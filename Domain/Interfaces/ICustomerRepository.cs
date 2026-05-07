using PruebaTecnicaFacundoTobioBack.Domain.Entities;

namespace PruebaTecnicaFacundoTobioBack.Domain.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        // Métodos específicos para Customer si hicieran falta
        Task<Customer?> GetCustomerWithInvoicesAsync(int id);
    }
}
