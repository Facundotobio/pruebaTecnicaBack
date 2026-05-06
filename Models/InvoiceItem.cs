using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaFacundoTobioBack.Models
{
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Navegación
        [System.Text.Json.Serialization.JsonIgnore] // para evitar que se serialicen en ambos sentidos y genere bucles infinitos
        public Invoice? Invoice { get; set; }
    }
}
