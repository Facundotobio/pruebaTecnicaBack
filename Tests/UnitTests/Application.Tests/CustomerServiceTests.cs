using Moq;
using FluentAssertions;
using PruebaTecnicaFacundoTobioBack.Application.Services;
using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;
using Xunit;

namespace Application.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCustomerResponseDto_WhenCustomerIsCreated()
        {
            // Arrange
            var customerDto = new CustomerCreateDto
            {
                Nombre = "Facundo Tobio",
                Email = "facundo@example.com",
                Direccion = "Calle Falsa 123",
                Telefono = "12345678"
            };

            // Act
            var result = await _customerService.CreateAsync(customerDto);

            // Assert
            result.Should().NotBeNull();
            result.Nombre.Should().Be(customerDto.Nombre);
            result.Email.Should().Be(customerDto.Email);
            
            _customerRepositoryMock.Verify(r => r.AddAsync(It.Is<Customer>(c => c.Nombre == customerDto.Nombre)), Times.Once);
            _customerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenCustomerIsUpdated()
        {
            // Arrange
            int customerId = 1;
            var existingCustomer = new Customer { CustomerId = customerId, Nombre = "Viejo Nombre" };
            var updateDto = new CustomerUpdateDto { Nombre = "Nuevo Nombre", Email = "nuevo@example.com" };

            _customerRepositoryMock.Setup(r => r.GetByIdAsync(customerId))
                .ReturnsAsync(existingCustomer);

            // Act
            var result = await _customerService.UpdateAsync(customerId, updateDto);

            // Assert
            result.Should().BeTrue();
            existingCustomer.Nombre.Should().Be(updateDto.Nombre);
            _customerRepositoryMock.Verify(r => r.Update(existingCustomer), Times.Once);
            _customerRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidOperationException_WhenCustomerHasInvoices()
        {
            // Arrange
            int customerId = 1;
            var customerWithInvoices = new Customer
            {
                CustomerId = customerId,
                Nombre = "Cliente con Deuda",
                Invoices = new List<Invoice> { new Invoice { InvoiceId = 99 } }
            };

            _customerRepositoryMock.Setup(r => r.GetCustomerWithInvoicesAsync(customerId))
                .ReturnsAsync(customerWithInvoices);

            // Act
            Func<Task> act = async () => await _customerService.DeleteAsync(customerId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("No se puede eliminar el cliente porque tiene facturas asociadas.");
            
            _customerRepositoryMock.Verify(r => r.Remove(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            // Arrange
            int customerId = 999;
            _customerRepositoryMock.Setup(r => r.GetCustomerWithInvoicesAsync(customerId))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _customerService.DeleteAsync(customerId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
