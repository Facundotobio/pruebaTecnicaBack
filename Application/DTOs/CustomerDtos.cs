using System.ComponentModel.DataAnnotations;

namespace PruebaTecnicaFacundoTobioBack.Application.DTOs
{
    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CustomerCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Telefono { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;
    }

    public class CustomerUpdateDto : CustomerCreateDto
    {
    }
}
