using PruebaTecnicaFacundoTobioBack.Application.DTOs;

namespace PruebaTecnicaFacundoTobioBack.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
        Task<CustomerResponseDto?> GetByIdAsync(int id);
        Task<CustomerResponseDto> CreateAsync(CustomerCreateDto customerDto);
        Task<bool> UpdateAsync(int id, CustomerUpdateDto customerDto);
        Task<bool> DeleteAsync(int id);
    }
}
