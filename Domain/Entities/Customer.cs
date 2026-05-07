using System.ComponentModel.DataAnnotations;
using static PruebaTecnicaFacundoTobioBack.Infrastructure.Data.Enums;

namespace PruebaTecnicaFacundoTobioBack.Domain.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required]
        public EntityStatus Estado { get; set; } = EntityStatus.Activo;

        // NavegaciÃ³n
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
