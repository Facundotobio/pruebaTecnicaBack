using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Data;
using PruebaTecnicaFacundoTobioBack.DTOs;
using PruebaTecnicaFacundoTobioBack.Interfaces;
using PruebaTecnicaFacundoTobioBack.Models;

namespace PruebaTecnicaFacundoTobioBack.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
        {
            return await _context.Customers
                .OrderBy(c => c.Nombre)
                .Select(c => new CustomerResponseDto
                {
                    CustomerId = c.CustomerId,
                    Nombre = c.Nombre,
                    Direccion = c.Direccion,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .ToListAsync();
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

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
                Email = customerDto.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

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
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null) return false;

            existingCustomer.Nombre = customerDto.Nombre;
            existingCustomer.Direccion = customerDto.Direccion;
            existingCustomer.Telefono = customerDto.Telefono;
            existingCustomer.Email = customerDto.Email;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null) return false;

            // Business rule: cannot delete customer with invoices
            if (customer.Invoices.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el cliente porque tiene facturas asociadas.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
