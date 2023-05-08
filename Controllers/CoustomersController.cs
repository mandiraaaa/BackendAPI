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

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCustomer")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        {
            var customer = await _mainDbContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound("customer not found");
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

        [HttpGet("ActiveCustomerCount")]
        public async Task<IActionResult> GetActiveCustomerCount()
        {
            var count = await _mainDbContext.Customers
                .Where(c => c.status == 1)
                .CountAsync();

            return Ok(count);
        }




        //client bill payment ...............................

        [HttpGet]
        [Route("{phoneNumber}")]
        [ActionName("GetCustomerByPhoneNumber")]
        public async Task<IActionResult> GetCustomerByPhoneNumber([FromRoute] string phoneNumber)
        {
            var customer = await _mainDbContext.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound("customer not found");
        }


        [HttpPost("ClientPayment")]
        public async Task<IActionResult> CustmomerPay([FromBody] Billing billing)
        {

            billing.Id = Guid.NewGuid();
            await _mainDbContext.Billings.AddAsync(billing);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = billing.Id }, billing);
        }

        //[HttpGet("GetAllClientPayments")]
        //public async Task<IActionResult> GetAllClientPayments()
        //{
        //    var customers = await _mainDbContext.Billings.ToListAsync();

        //    return Ok(customers);
        //}

        [HttpGet("GetCustomerPayments/{phoneNumber}")]
        public async Task<IActionResult> GetCustomerPayments(string phoneNumber)
        {
            var customer = await _mainDbContext.Customers.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

            if (customer != null)
            {
                var customerPayments = await _mainDbContext.Billings.Where(x => x.CustomerPhoneNumber == customer.PhoneNumber).ToListAsync();

                return Ok(customerPayments);
            }

            return NotFound("Customer not found");
        }


        [HttpGet("getpaymentbyid/{id}")]
        //[Route("{id:guid}")]
        //[ActionName("GetPayment")]
        public async Task<IActionResult> GetPayment([FromRoute] Guid id)
        {
            var customer = await _mainDbContext.Billings.FirstOrDefaultAsync(x => x.Id == id);
            if (customer != null)
            {
                return Ok(customer);
            }
            return NotFound("payment not found");
        }

        [HttpPut("updatepayment/{id}")]
        //[Route("{id:guid}")]
        public async Task<IActionResult> UpdatePayment([FromRoute] Guid id, [FromBody] Billing billing)
        {
            var existingPayment = await _mainDbContext.Billings.FirstOrDefaultAsync(x => x.Id == id);
            if (existingPayment != null)
            {
                existingPayment.CustomerPhoneNumber = billing.CustomerPhoneNumber;
                existingPayment.installmentName = billing.installmentName;
                existingPayment.Amount = billing.Amount;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingPayment);
            }

            return NotFound("Payment not found");
        }

        [HttpDelete("deletepayment/{id}")]
        //[Route("{id:guid}")]
        public async Task<IActionResult> DeletePayment([FromRoute] Guid id)
        {
            var existingCustomer = await _mainDbContext.Billings.FirstOrDefaultAsync(x => x.Id == id);
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
