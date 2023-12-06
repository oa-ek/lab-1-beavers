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
    public class DeveloperApiController : ControllerBase
    {
        private readonly IRepository<Developer> _developersRepository;
    
        public DeveloperApiController(IRepository<Developer> developersRepository)
        {
            _developersRepository = developersRepository;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _developersRepository.GetAllEntitiesAsync());
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            return Ok(await _developersRepository.GetEntityByIdAsync(id));
        }
    }
}
