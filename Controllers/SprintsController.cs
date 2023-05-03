using BackendAPI.Data;
using BackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SprintsController : Controller
    {
        private readonly MainDbContext _mainDbContext;

        public SprintsController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        // get all sprints
        [HttpGet]
        public async Task<IActionResult> GetAllSprints()
        {
            var sprints = await _mainDbContext.Sprints.ToListAsync();

            return Ok(sprints);
        }

        // get single sprint
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetSprint")]
        public async Task<IActionResult> GetSprint([FromRoute] Guid id)
        {
            var sprint = await _mainDbContext.Sprints.FirstOrDefaultAsync(x => x.Id == id);
            if (sprint != null)
            {
                return Ok(sprint);
            }
            return NotFound("Sprint not found");
        }

        // add sprint
        [HttpPost]
        public async Task<IActionResult> AddSprint([FromBody] Sprint sprint)
        {
            sprint.Id = Guid.NewGuid();
            await _mainDbContext.Sprints.AddAsync(sprint);
            await _mainDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSprint), new { id = sprint.Id }, sprint);
        }

        // update an sprint
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateSprint([FromRoute] Guid id, [FromBody] Sprint sprint)
        {
            var existingSprint = await _mainDbContext.Sprints.FirstOrDefaultAsync(x => x.Id == id);
            if (existingSprint != null)
            {
                existingSprint.Id = sprint.Id;
                existingSprint.Name = sprint.Name;
                existingSprint.StartDate = sprint.StartDate;
                existingSprint.DueDate = sprint.DueDate;
                existingSprint.Status = sprint.Status;
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingSprint);
            }

            return NotFound("Sprint not found");
        }

        // delete a sprint
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteSprint([FromRoute] Guid id)
        {
            var existingSprint = await _mainDbContext.Sprints.FirstOrDefaultAsync(x => x.Id == id);
            if (existingSprint != null)
            {
                _mainDbContext.Remove(existingSprint);
                await _mainDbContext.SaveChangesAsync();
                return Ok(existingSprint);
            }

            return NotFound("Sprint not found");
        }
    }
}
