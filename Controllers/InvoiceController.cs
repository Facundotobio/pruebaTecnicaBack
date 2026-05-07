using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaFacundoTobioBack.Data;
using PruebaTecnicaFacundoTobioBack.Models;
using static PruebaTecnicaFacundoTobioBack.Data.@enum;

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

        // POST: api/Invoice - Crear Factura
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

            invoice.Estado = InvoiceStatus.Activo;
            invoice.Fecha = DateTime.UtcNow;

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Retornar factura completa
            var facturaCreada = await _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == invoice.InvoiceId);

            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.InvoiceId }, facturaCreada);
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            // Usamos AsNoTracking() para mejorar el rendimiento en lecturas
            var invoices = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .Where(i => i.Estado == InvoiceStatus.Activo)
                .AsNoTracking()
                .ToListAsync();

            return Ok(invoices);
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceId == id && i.Estado == InvoiceStatus.Activo);

            if (invoice == null) return NotFound("Factura no encontrada.");

            return Ok(invoice);
        }
    }
}
