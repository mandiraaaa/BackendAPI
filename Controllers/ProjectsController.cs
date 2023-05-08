using BackendAPI.Data;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly MainDbContext _mainDbContext;

        public ProjectsController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        // get all projects
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _mainDbContext.Projects.ToListAsync();

            return Ok(projects);
        }

        // get single employee
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetProject")]
        public async Task<IActionResult> GetProject([FromRoute] Guid id)
        {
            var project = await _mainDbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (project != null)
            {
                return Ok(project);
            }
            return NotFound("This project not found");
        }

        // add employee
        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] Project project)
        {
            project.Id = Guid.NewGuid();
            await _mainDbContext.Projects.AddAsync(project);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // update an employee
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateProject([FromRoute] Guid id, [FromBody] Project project)
        {
            var existingProject = await _mainDbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProject != null)
            {
                existingProject.Id = project.Id;
                existingProject.Name = project.Name;
                existingProject.Client = project.Client;
                existingProject.Duration = project.Duration;
                existingProject.Status = project.Status;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingProject);
            }

            return NotFound("Project not found");
        }

        // delete a card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
        {
            var existingProject = await _mainDbContext.Projects.FirstOrDefaultAsync(x => x.Id == id);
            if (existingProject != null)
            {
                _mainDbContext.Remove(existingProject);
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingProject);
            }

            return NotFound("Project not found");
        }


        [HttpGet("ActiveProjectCount")]
        public async Task<IActionResult> GetActiveProjectCount()
        {
            var count = await _mainDbContext.Projects
                .Where(c => c.Status == 1)
                .CountAsync();

            return Ok(count);
        }

    }
}
