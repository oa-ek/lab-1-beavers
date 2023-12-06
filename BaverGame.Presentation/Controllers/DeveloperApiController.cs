using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using BaverGame.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeveloperApiController : ControllerBase
    {
        private readonly IRepository<Developer> _developersRepository;

        public DeveloperApiController(IRepository<Developer> developersRepository)
        {
            _developersRepository = developersRepository;
        }

        // GET: api/Developer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _developersRepository.GetAllEntitiesAsync());
        }

        // GET: api/Developer/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var developer = await _developersRepository.GetEntityByIdAsync(id);
            if (developer == null)
                return NotFound();

            return Ok(developer);
        }

        // POST: api/Developer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeveloperDto developerDto)
        {
            if (string.IsNullOrWhiteSpace(developerDto.DeveloperName))
                return BadRequest("Developer name is required.");

            var developer = new Developer
            {
                DeveloperName = developerDto.DeveloperName
            };
            await _developersRepository.AddNewEntityAsync(developer);

            return CreatedAtAction(nameof(Get), new { id = developer.DeveloperId }, developer);
        }

        // PUT: api/Developer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DeveloperDto developerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var developer = await _developersRepository.GetEntityByIdAsync(id);
            if (developer == null)
                return NotFound();

            developer.DeveloperName = developerDto.DeveloperName;
            _developersRepository.UpdateExistingEntity(developer);

            return NoContent();
        }

        // DELETE: api/Developer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var developer = await _developersRepository.GetEntityByIdAsync(id);
            if (developer == null)
                return NotFound();

            _developersRepository.RemoveExistingEntity(developer);
            return NoContent();
        }
    }
}
