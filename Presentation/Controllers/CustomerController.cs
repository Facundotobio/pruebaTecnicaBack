using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaFacundoTobioBack.Application.DTOs;
using PruebaTecnicaFacundoTobioBack.Application.Interfaces;

namespace PruebaTecnicaFacundoTobioBack.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // POST: api/Customer
        [HttpPost]
        public async Task<ActionResult<CustomerResponseDto>> PostCustomer(CustomerCreateDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.CreateAsync(customerDto);

            return CreatedAtAction(nameof(GetCustomers), new { id = result.CustomerId }, result);
        }

        // GET: api/Customer - Listado completo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        // GET: api/Customer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomer(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // PUT: api/Customer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, [FromBody] CustomerUpdateDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _customerService.UpdateAsync(id, customerDto);

            if (!success)
            {
                return NotFound($"No se encontró el cliente con ID {id}");
            }

            return NoContent();
        }

        // DELETE: api/Customer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var success = await _customerService.DeleteAsync(id);
                if (!success) return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
