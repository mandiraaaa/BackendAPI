using BackendAPI.Data;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;
using System;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IConfiguration configuration;

        public TodosController(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _mainDbContext = mainDbContext;
            this.configuration = configuration;
        }

        // get all tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var sprints = await _mainDbContext.Sprints.ToListAsync();

            return Ok(sprints);
        }

        // get single task
        [HttpGet("getalltodos/{id:guid}")]
        //[Route("{id:guid}")]
        //[ActionName("GetAllTodo")]
        public async Task<IActionResult> GetTodo( Guid id)
        {
            var todo = await _mainDbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (todo != null)
            {
                return Ok(todo);
            }
            return NotFound("Task not found");
        }

        [HttpGet("EmployeeDrop")]
        public async Task<IActionResult> EmployeeDrop()
        {
            //using IDbConnection conn = new SqlConnection(this.configuration.GetConnectionString("DBConnection"));
            //var employeeid = await _mainDbContext.QueryAsync("BM.spGetJobDetailsandBArgeAllocationGrid", commandType: CommandType.StoredProcedure);


            using (var connection = new SqlConnection(_mainDbContext.Database.GetConnectionString()))
            {
                connection.Open();
                var employeedrop = await connection.QueryAsync<dynamic>("GetEmployeedrop", commandType: CommandType.StoredProcedure);

                if (employeedrop != null)
                {
                    return Ok(employeedrop);
                }
                else
                {
                    return NotFound("Employee not found");
                }
            }
            //    var Employeeid = await _mainDbContext.Employees
            //.FromSqlRaw("SELECT T.* from Todos T INNER JOIN (SELECT * FROM Employees WHERE User_Id=@userid) u ON T.AssignedTo= u.Id", new SqlParameter("@userid", userid))
            //.ToListAsync();


        }

        // add task
        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody] Todo todo)
        {

            todo.Id = Guid.NewGuid();
            await _mainDbContext.Todos.AddAsync(todo);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // update a task
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateTodo([FromRoute] Guid id, [FromBody] Todo todo)
        {
            var existingTodo = await _mainDbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTodo != null)
            {
                existingTodo.Id = todo.Id;
                existingTodo.Name = todo.Name;
                existingTodo.StartDate = todo.StartDate;
                existingTodo.DueDate = todo.DueDate;
                existingTodo.AssignedTo = todo.AssignedTo;
                existingTodo.Priority = todo.Priority;
                existingTodo.Status = todo.Status;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingTodo);
            }

            return NotFound("Task not found");
        }

        // delete a task
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] Guid id)
        {
            var existingTodo = await _mainDbContext.Todos.FirstOrDefaultAsync(x => x.Id == id);
            if (existingTodo != null)
            {
                _mainDbContext.Remove(existingTodo);
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingTodo);
            }

            return NotFound("Task not found");
        }

        [HttpGet("gettodosbyempid/{Empid}")]
        //[Route("{Empid}")]
        //[ActionName("GetTodo")]
        public async Task<IActionResult> Todo(string Empid)
        {
            var todolist =await _mainDbContext.Todos
         .FromSqlRaw("SELECT * FROM Todos WHERE Status =10 AND AssignedTo = @Empid", new SqlParameter("@Empid", Empid))
         .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }

        [HttpGet("getInprogressByempid/{Empid}")]
        //[Route("{Empid}")]
        //[ActionName("GetTodo")]
        public async Task<IActionResult> Inprogress(string Empid)
        {
            var todolist = await _mainDbContext.Todos
         .FromSqlRaw("SELECT * FROM Todos WHERE Status =20 AND AssignedTo = @Empid", new SqlParameter("@Empid", Empid))
         .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }

        [HttpGet("getDoneByempid/{Empid}")]
        //[Route("{Empid}")]
        //[ActionName("GetTodo")]
        public async Task<IActionResult> Done(string Empid)
        {
            var todolist = await _mainDbContext.Todos
         .FromSqlRaw("SELECT * FROM Todos WHERE Status =30 AND AssignedTo = @Empid", new SqlParameter("@Empid", Empid))
         .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }

        //..................................mobile app api.........................................................................


        [HttpPost("EmpDailyTask")]
        public async Task<IActionResult> EmpDayTodo([FromBody] Perfomence perfomence)
        {

            perfomence.id = Guid.NewGuid();
            await _mainDbContext.perfomences.AddAsync(perfomence);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = perfomence.id }, perfomence);
        }

        // employee daily all task table
        [HttpGet("AllEmpTodobyDate")]
        public async Task<IActionResult> GetAllEmpDayTodo()
        {
            
            var perfomences = await _mainDbContext.perfomences
        .FromSqlRaw("SELECT * FROM perfomences WHERE Date = CAST( GETDATE() AS Date )")
        .ToListAsync();
            if (perfomences != null)
            {
                return Ok(perfomences);
            }
            return NotFound("Task not found");
        }

        [HttpGet("EmployeebyUserID/{userid}")]
        public async Task<IActionResult> GetEmployeebyUserID(string userid)
        {
            //using IDbConnection conn = new SqlConnection(this.configuration.GetConnectionString("DBConnection"));
            //var employeeid = await _mainDbContext.QueryAsync("BM.spGetJobDetailsandBArgeAllocationGrid", commandType: CommandType.StoredProcedure);

            
            using (var connection = new SqlConnection(_mainDbContext.Database.GetConnectionString()))
            {
                connection.Open();
                var para = new DynamicParameters();
                para.Add("@userid", userid);
                var employeeid = await connection.QueryAsync<dynamic>("GetTodsByUserId", para, commandType: CommandType.StoredProcedure);
                
                if (employeeid != null)
                {
                    return Ok(employeeid);
                }
                else
                {
                    return NotFound("Task not found");
                }
            }
            //    var Employeeid = await _mainDbContext.Employees
            //.FromSqlRaw("SELECT T.* from Todos T INNER JOIN (SELECT * FROM Employees WHERE User_Id=@userid) u ON T.AssignedTo= u.Id", new SqlParameter("@userid", userid))
            //.ToListAsync();
           
           
        }
    }
}
