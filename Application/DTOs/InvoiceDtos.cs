using System.ComponentModel.DataAnnotations;
using static PruebaTecnicaFacundoTobioBack.Infrastructure.Data.@enum;

namespace PruebaTecnicaFacundoTobioBack.Application.DTOs
{
    public class InvoiceResponseDto
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Numero { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public List<InvoiceItemResponseDto> Items { get; set; } = new();
    }

    public class InvoiceItemResponseDto
    {
        public int InvoiceItemId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class InvoiceCreateDto
    {
        [Required]
        public int CustomerId { get; set; }

        [MaxLength(20)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "La factura debe tener al menos un item")]
        public List<InvoiceItemCreateDto> Items { get; set; } = new();
    }

    public class InvoiceItemCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }
    }
}
