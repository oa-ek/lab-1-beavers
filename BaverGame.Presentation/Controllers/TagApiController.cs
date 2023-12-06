using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TagApiController : ControllerBase
    {
        private readonly IRepository<Tag> _tagsRepository;
    
        public TagApiController(IRepository<Tag> tagsRepository)
        {
            _tagsRepository = tagsRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tagsRepository.GetAllEntitiesAsync());
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            return Ok(await _tagsRepository.GetEntityByIdAsync(id));
        }
    }
}
