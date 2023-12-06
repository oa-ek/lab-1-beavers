using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using BaverGame.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BaverGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherApiController : ControllerBase
    {
        private readonly IRepository<Publisher> _publishersRepository;

        public PublisherApiController(IRepository<Publisher> publishersRepository)
        {
            _publishersRepository = publishersRepository;
        }

        // GET: api/Publisher
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _publishersRepository.GetAllEntitiesAsync());
        }

        // GET: api/Publisher/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var publisher = await _publishersRepository.GetEntityByIdAsync(id);
            if (publisher == null)
                return NotFound();

            return Ok(publisher);
        }

        // POST: api/Publisher
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublisherDto publisherDto)
        {
            if (string.IsNullOrWhiteSpace(publisherDto.PublisherName))
                return BadRequest("Publisher name is required.");

            var publisher = new Publisher
            {
                PublisherName = publisherDto.PublisherName
            };
            await _publishersRepository.AddNewEntityAsync(publisher);

            return CreatedAtAction(nameof(Get), new { id = publisher.PublisherId }, publisher);
        }

        // PUT: api/Publisher/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PublisherDto publisherDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publisher = await _publishersRepository.GetEntityByIdAsync(id);
            if (publisher == null)
                return NotFound();

            publisher.PublisherName = publisherDto.PublisherName;
            _publishersRepository.UpdateExistingEntity(publisher);

            return NoContent();
        }

        // DELETE: api/Publisher/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var publisher = await _publishersRepository.GetEntityByIdAsync(id);
            if (publisher == null)
                return NotFound();

            _publishersRepository.RemoveExistingEntity(publisher);
            return NoContent();
        }
    }
}
