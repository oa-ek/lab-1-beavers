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
    public class PublisherApiController : ControllerBase
    {
        private readonly IRepository<Publisher> _publishersRepository;
    
        public PublisherApiController(IRepository<Publisher> publishersRepository)
        {
            _publishersRepository = publishersRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _publishersRepository.GetAllEntitiesAsync());
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            return Ok(await _publishersRepository.GetEntityByIdAsync(id));
        }
    }
}
