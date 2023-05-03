using BackendAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using BackendAPI.Models;

namespace BackendAPI.Controllers
{
    public class CoustomersController : Controller
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IConfiguration configuration;

        public CoustomersController(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _mainDbContext = mainDbContext;
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            var customers = await _mainDbContext.Customers.ToListAsync();

            return Ok(customers);
        }

        [HttpPost("Addclient")]
        public async Task<IActionResult> Addclient([FromBody] Customer customer)
        {

            customer.Id = Guid.NewGuid();
            await _mainDbContext.Customers.AddAsync(customer);
            await _mainDbContext.SaveChangesAsync();

           return Ok();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] Customer customer)
        {
            var existingCustomer = await _mainDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.project = customer.project;
                existingCustomer.Duration = customer.Duration;
                existingCustomer.Package = customer.Package;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.status = customer.status;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingCustomer);
            }

            return NotFound("Customer not found");
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            var existingCustomer = await _mainDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCustomer != null)
            {
                _mainDbContext.Remove(existingCustomer);
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingCustomer);
            }

            return NotFound("Customer not found");
        }
    }
}
