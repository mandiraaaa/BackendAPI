using BackendAPI.Data;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly MainDbContext _mainDbContext;

        public EmployeesController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        // get all employees
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _mainDbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        // get single employee
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _mainDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                return Ok(employee);
            }
            return NotFound("Employee not found");
        }

        // add employee
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequestModel employee)
        {
            var userId = _mainDbContext.Users.Where(u => u.Name == employee.Name).Select(u => u.Id).FirstOrDefault();
            var employeeobj = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = employee.Name,
                JobRole = employee.JobRole,
                JoinedDate = employee.JoinedDate,
                Salary = employee.Salary,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                User_Id = userId.ToString()
            };
            
   
            await _mainDbContext.Employees.AddAsync(employeeobj);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new {id = employeeobj.Id }, employee);
        }

        //update an employee
       [HttpPut]
       [Route("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] Employee employee)
        {
            var existingEmployee = await _mainDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEmployee != null)
            {
                existingEmployee.Name = employee.Name;
                existingEmployee.JobRole = employee.JobRole;
                existingEmployee.JoinedDate = employee.JoinedDate;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.Address = employee.Address;
                existingEmployee.PhoneNumber = employee.PhoneNumber;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }

            return NotFound("Employee not found");
        }

        //[HttpPut]
        //[Route("{id:guid}")]
        //public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, [FromBody] Employee employee)
        //{
        //    var existingEmployee = await _mainDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        //    if (existingEmployee != null)
        //    {
        //        existingEmployee.Name = employee.Name;
        //        existingEmployee.JobRole = employee.JobRole;
        //        existingEmployee.JoinedDate = employee.JoinedDate;
        //        existingEmployee.Salary = employee.Salary;
        //        existingEmployee.Address = employee.Address;
        //        existingEmployee.PhoneNumber = employee.PhoneNumber;
        //        await _mainDbContext.SaveChangesAsync();
        //        return Ok(existingEmployee);
        //    }

        //    return NotFound("Employee not found");
        //}


        // delete a card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var existingEmployee = await _mainDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (existingEmployee != null)
            {
                _mainDbContext.Remove(existingEmployee);
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingEmployee);
            }

            return NotFound("Employee not found");
        }

        [HttpGet]
        [Route("employee-count")]
        public async Task<IActionResult> GetEmployeeCount()
        {
            int count = await _mainDbContext.Employees.CountAsync();
            return Ok(new { count });
        }

    }
}
