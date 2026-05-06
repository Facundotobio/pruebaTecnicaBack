using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaFacundoTobioBack.Models
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

        // Navegación
        [System.Text.Json.Serialization.JsonIgnore] // para evitar que se serialicen en ambos sentidos y genere bucles infinitos
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
