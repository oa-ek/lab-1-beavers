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
    public class TagApiController : ControllerBase
    {
        private readonly IRepository<Tag> _tagsRepository;

        public TagApiController(IRepository<Tag> tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }

        // GET: api/Tag
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tagsRepository.GetAllEntitiesAsync());
        }

        // GET: api/Tag/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var tag = await _tagsRepository.GetEntityByIdAsync(id);
            if (tag == null)
                return NotFound();

            return Ok(tag);
        }

        // POST: api/Tag
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagDto tagDto)
        {
            if (string.IsNullOrWhiteSpace(tagDto.TagName))
                return BadRequest("Tag name is required.");

            var tag = new Tag
            {
                TagName = tagDto.TagName
            };
            await _tagsRepository.AddNewEntityAsync(tag);

            return CreatedAtAction(nameof(Get), new { id = tag.TagId }, tag);
        }

        // PUT: api/Tag/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TagDto tagDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tag = await _tagsRepository.GetEntityByIdAsync(id);
            if (tag == null)
                return NotFound();

            tag.TagName = tagDto.TagName;
            _tagsRepository.UpdateExistingEntity(tag);

            return NoContent();
        }

        // DELETE: api/Tag/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _tagsRepository.GetEntityByIdAsync(id);
            if (tag == null)
                return NotFound();

            _tagsRepository.RemoveExistingEntity(tag);
            return NoContent();
        }
    }
}
