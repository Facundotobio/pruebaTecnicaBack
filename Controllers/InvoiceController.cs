using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Data;
using PruebaTecnicaFacundoTobioBack.Models;

namespace PruebaTecnicaFacundoTobioBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Invoice - Crear Factura con Items
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            if (invoice == null || invoice.Items == null || !invoice.Items.Any())
            {
                return BadRequest("La factura debe contener al menos un item.");
            }

            if (invoice.CustomerId <= 0)
            {
                return BadRequest("Debe especificar un CustomerId válido.");
            }

            // Generar número de factura
            invoice.Numero = $"FACT-{DateTime.UtcNow:yyyyMMddHHmmss}";

            // Calcular total
            invoice.Total = invoice.Items.Sum(i => i.Cantidad * i.PrecioUnitario);
            invoice.Fecha = DateTime.UtcNow;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Retornar factura completa
            var facturaCreada = await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoice.InvoiceId);

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, facturaCreada);
        }

        // GET: api/Invoice/{id} - Obtener factura por ID (útil para verificar)
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
                return NotFound("Factura no encontrada.");

            return invoice;
        }
    }
}
