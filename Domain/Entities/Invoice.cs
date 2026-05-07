using System.ComponentModel.DataAnnotations;
using static PruebaTecnicaFacundoTobioBack.Infrastructure.Data.Enums;

namespace PruebaTecnicaFacundoTobioBack.Domain.Entities
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        public decimal Total { get; set; }

        [Required]
        public EntityStatus Estado { get; set; } = EntityStatus.Activo;

        // NavegaciÃ³n
        public Customer? Customer { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
