using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Application.Interfaces;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;
using static PruebaTecnicaFacundoTobioBack.Infrastructure.Data.@enum;

namespace PruebaTecnicaFacundoTobioBack.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers
                .Where(c => c.Estado == EntityStatus.Activo)
                .OrderBy(c => c.Nombre)
                .Select(c => new CustomerResponseDto
            {
                CustomerId = c.CustomerId,
                Nombre = c.Nombre,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Email = c.Email
            });
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null || customer.Estado == EntityStatus.Inactivo) return null;

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Nombre = customer.Nombre,
                Direccion = customer.Direccion,
                Telefono = customer.Telefono,
                Email = customer.Email
            };
        }

        public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto customerDto)
        {
            var customer = new Customer
            {
                Nombre = customerDto.Nombre,
                Direccion = customerDto.Direccion,
                Telefono = customerDto.Telefono,
                Email = customerDto.Email,
                Estado = EntityStatus.Activo
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return new CustomerResponseDto
            {
                CustomerId = customer.CustomerId,
                Nombre = customer.Nombre,
                Direccion = customer.Direccion,
                Telefono = customer.Telefono,
                Email = customer.Email
            };
        }

        public async Task<bool> UpdateAsync(int id, CustomerUpdateDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null || existingCustomer.Estado == EntityStatus.Inactivo) return false;

            existingCustomer.Nombre = customerDto.Nombre;
            existingCustomer.Direccion = customerDto.Direccion;
            existingCustomer.Telefono = customerDto.Telefono;
            existingCustomer.Email = customerDto.Email;

            _customerRepository.Update(existingCustomer);
            await _customerRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerWithInvoicesAsync(id);
            if (customer == null || customer.Estado == EntityStatus.Inactivo) return false;

            // Regla de negocio: No se puede eliminar si tiene facturas (aunque sea borrado lógico, para mantener integridad si se requiere)
            if (customer.Invoices.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene facturas asociadas.");
            }

            customer.Estado = EntityStatus.Inactivo;
            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
            return true;
        }
    }
}
