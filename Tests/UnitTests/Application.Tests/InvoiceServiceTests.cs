using Moq;
using FluentAssertions;
using PruebaTecnicaFacundoTobioBack.Application.Services;
using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Domain.Entities;
using PruebaTecnicaFacundoTobioBack.Domain.Interfaces;
using Xunit;
using static PruebaTecnicaFacundoTobioBack.Infrastructure.Data.Enums;

namespace Application.Tests
{
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _invoiceService = new InvoiceService(_invoiceRepositoryMock.Object, _customerRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCalculateTotalCorrectly_WhenAddingMultipleItems()
        {
            // Arrange
            var invoiceDto = new InvoiceCreateDto
            {
                CustomerId = 1,
                Numero = "FAC-001",
                Items = new List<InvoiceItemCreateDto>
                {
                    new InvoiceItemCreateDto { Descripcion = "Item 1", Cantidad = 2, PrecioUnitario = 50 },
                    new InvoiceItemCreateDto { Descripcion = "Item 2", Cantidad = 1, PrecioUnitario = 150.50m }
                }
            };

            // Mocking customer existence
            _customerRepositoryMock.Setup(r => r.GetByIdAsync(invoiceDto.CustomerId))
                .ReturnsAsync(new Customer { CustomerId = 1 });
            
            decimal expectedTotal = 250.50m;

            // Act
            var result = await _invoiceService.CreateAsync(invoiceDto);

            // Assert
            result.Total.Should().Be(expectedTotal);
            _invoiceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var invoiceDto = new InvoiceCreateDto { CustomerId = 99 };
            _customerRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Customer?)null);

            // Act
            Func<Task> act = async () => await _invoiceService.CreateAsync(invoiceDto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("No se encontrÃ³ el cliente con ID 99");
            
            _invoiceRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenInvoiceExists()
        {
            // Arrange
            int invoiceId = 10;
            var invoice = new Invoice
            {
                InvoiceId = invoiceId,
                CustomerId = 1,
                Numero = "FAC-10",
                Total = 500,
                Estado = EntityStatus.Activo,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem { Descripcion = "Test Item", Cantidad = 1, PrecioUnitario = 500 }
                }
            };

            _invoiceRepositoryMock.Setup(r => r.GetInvoiceWithItemsAsync(invoiceId))
                .ReturnsAsync(invoice);

            // Act
            var result = await _invoiceService.GetByIdAsync(invoiceId);

            // Assert
            result.Should().NotBeNull();
            result!.Estado.Should().Be("Activo");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenInvoiceExists()
        {
            // Arrange
            int invoiceId = 1;
            var invoice = new Invoice { InvoiceId = invoiceId };
            
            _invoiceRepositoryMock.Setup(r => r.GetByIdAsync(invoiceId))
                .ReturnsAsync(invoice);

            // Act
            var result = await _invoiceService.DeleteAsync(invoiceId);

            // Assert
            result.Should().BeTrue();
            _invoiceRepositoryMock.Verify(r => r.Remove(invoice), Times.Once);
        }
    }
}
