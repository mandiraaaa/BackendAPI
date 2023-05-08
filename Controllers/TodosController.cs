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
                //existingTodo.Id = todo.Id;
                existingTodo.Name = todo.Name;
                existingTodo.StartDate = todo.StartDate;
                existingTodo.DueDate = todo.DueDate;
                existingTodo.Assignee = todo.Assignee;
                existingTodo.Priority = todo.Priority;
                existingTodo.Status = todo.Status;
                existingTodo.sprint_Name = todo.sprint_Name;
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
        //get status 10 filtered employee.....................
        //[HttpGet("gettodosbyusername/{username}")]
        ////[Route("{Empid}")]
        ////[ActionName("GetTodo")]
        //public async Task<IActionResult> Todo(string username)
        //{
        //    var todolist =await _mainDbContext.Todos
        // .FromSqlRaw("SELECT * FROM Todos WHERE Status =10 AND Assignee = @username", new SqlParameter("@username", username))
        // .ToListAsync();
        //    if (todolist != null)
        //    {
        //        return Ok(todolist);
        //    }
        //    return NotFound("Task not found");
        //}


        [HttpGet("getalltodos")]
        public async Task<IActionResult> Todo()
        {
            var todolist = await _mainDbContext.Todos
                .FromSqlRaw("SELECT * FROM Todos WHERE Status = 10")
                .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }

        // filtered employee status 20.....................
        //[HttpGet("getInprogressByempid/{Empid}")]
        ////[Route("{Empid}")]
        ////[ActionName("GetTodo")]
        //public async Task<IActionResult> Inprogress(string Empid)
        //{
        //    var todolist = await _mainDbContext.Todos
        // .FromSqlRaw("SELECT * FROM Todos WHERE Status =20 AND AssignedTo = @Empid", new SqlParameter("@Empid", Empid))
        // .ToListAsync();
        //    if (todolist != null)
        //    {
        //        return Ok(todolist);
        //    }
        //    return NotFound("Task not found");
        //}

        [HttpGet("getInprogress")]
        public async Task<IActionResult> Inprogress()
        {
            var todolist = await _mainDbContext.Todos
                .FromSqlRaw("SELECT * FROM Todos WHERE Status = 20")
                .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }


        //filtered employee when status 30....................
        //[HttpGet("getDoneByempid/{Empid}")]
        ////[Route("{Empid}")]
        ////[ActionName("GetTodo")]
        //public async Task<IActionResult> Done(string Empid)
        //{
        //    var todolist = await _mainDbContext.Todos
        // .FromSqlRaw("SELECT * FROM Todos WHERE Status =30 AND AssignedTo = @Empid", new SqlParameter("@Empid", Empid))
        // .ToListAsync();
        //    if (todolist != null)
        //    {
        //        return Ok(todolist);
        //    }
        //    return NotFound("Task not found");
        //}

        [HttpGet("getDone")]
        public async Task<IActionResult> Done()
        {
            var todolist = await _mainDbContext.Todos
                .FromSqlRaw("SELECT * FROM Todos WHERE Status = 30")
                .ToListAsync();
            if (todolist != null)
            {
                return Ok(todolist);
            }
            return NotFound("Task not found");
        }
//...........................dashboard..................................................

        [HttpGet("GetHighPriorityTaskCount")]
        public async Task<IActionResult> GetHighPriorityTaskCount()
        {
            var highPriorityCount = await _mainDbContext.Todos.CountAsync(x => x.Priority == "high");

            return Ok(highPriorityCount);
        }

        [HttpGet("GetLowPriorityTaskCount")]
        public async Task<IActionResult> GetLowPriorityTaskCount()
        {
            var highPriorityCount = await _mainDbContext.Todos.CountAsync(x => x.Priority == "low");

            return Ok(highPriorityCount);
        }

        [HttpGet("GetNormalPriorityTaskCount")]
        public async Task<IActionResult> GetNormalPriorityTaskCount()
        {
            var highPriorityCount = await _mainDbContext.Todos.CountAsync(x => x.Priority == "normal");

            return Ok(highPriorityCount);
        }



        [HttpGet]
        [Route("todos/count")]
        public async Task<IActionResult> GetTodoCount()
        {
            var count = await _mainDbContext.Todos.CountAsync(x => x.Status == 10);
            return Ok(count);
        }

        [HttpGet]
        [Route("inprogress/count")]
        public async Task<IActionResult> GetInProgressCount()
        {
            var count = await _mainDbContext.Todos.CountAsync(x => x.Status == 20);
            return Ok(count);
        }

        [HttpGet]
        [Route("done/count")]
        public async Task<IActionResult> GetDoneCount()
        {
            var count = await _mainDbContext.Todos.CountAsync(x => x.Status == 30);
            return Ok(count);
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

        [HttpGet("EmployeebyUserName/{name}")]
        public async Task<IActionResult> GetEmployeebyUserID(string name)
        {
            //using IDbConnection conn = new SqlConnection(this.configuration.GetConnectionString("DBConnection"));
            //var employeeid = await _mainDbContext.QueryAsync("BM.spGetJobDetailsandBArgeAllocationGrid", commandType: CommandType.StoredProcedure);

            
            using (var connection = new SqlConnection(_mainDbContext.Database.GetConnectionString()))
            {
                connection.Open();
                var para = new DynamicParameters();
                para.Add("@name", name);
                var Name = await connection.QueryAsync<dynamic>("GetTodsByUserName", para, commandType: CommandType.StoredProcedure);
                
                if (Name != null)
                {
                    return Ok(Name);
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
