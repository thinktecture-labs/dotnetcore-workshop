using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GrpcService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
    // https://localhost:5001/akte
    // GET    https://localhost:5001/akte -- Liste von akte
    // GET    https://localhost:5001/akte/{id} -- Einzelnen akte 
    // POST   https://localhost:5001/akte -- Legt eine neue akte an
    // POST   https://localhost:5001/akte/lock -- Erzeugt eine sperre auf der akte
    // PUT    https://localhost:5001/akte/{id} -- Aktuallisiert eine akte
    // DELETE https://localhost:5001/akte/{name} -- Loescht eine akte
    [Route("[controller]")]
    [ApiController]
    public class AkteController : Controller
    {
        private readonly ILogger<AkteController> _logger;
        private readonly AkteDbContext _dbContext;

        public AkteController(ILogger<AkteController> logger, AkteDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        
        // GET https://localhost:5001/akte
        /// <summary>
        /// Laede eine liste von akten aus der datenbank
        /// </summary>
        /// <returns>
        /// eine liste von akten.
        /// </returns>
        /// <response code="418">If tea time (at noon!)</response>    
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MvcAkte>))]
        [ProducesResponseType(418)]
        public ActionResult<IEnumerable<AkteResponseModel>> List()
        {
            if (DateTime.Now.Hour == 12)
            {
                return StatusCode(418); // 418 == I'am a teapot; 
            }
            
            var akten = _dbContext.Akten.Include(a => a.Vorgaenge).ToList();
            return akten.Select(a => new AkteResponseModel()
                {Id = a.AktenNummer, Name = a.Name, CreatedDate = a.CreatedDate})
                .ToList();
        }
        
        // GET https://localhost:5001/akte/{id}
        [HttpGet("{id}")]
        public ActionResult<AkteResponseModel> GetById(Guid id)
        {
            var akte = _dbContext.Akten.FirstOrDefault(a => a.AktenNummer == id);
            if (akte == null)
            {
                return NotFound();
            }

            return new AkteResponseModel() {Id = akte.AktenNummer, Name = akte.Name, CreatedDate = akte.CreatedDate};
        }
        
        // POST https://localhost:5001/akte
        [HttpPost]
        public IActionResult Create([FromBody]AkteInputModel input)
        {
            var akte = new Akte()
            {
                AktenNummer = Guid.NewGuid(),
                Name = input.Name
            };

            _dbContext.Akten.Add(akte);
            _dbContext.SaveChanges();
            
            // Akte anlegen
            // return CreatedAtAction(akte);
            return CreatedAtAction(nameof(GetById), new {id = akte.AktenNummer});
        }
        
        // PUT https://localhost:5001/akte/{id}?name=Test
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, AkteInputModel input)
        {
            // var akte = _dbContext.Akten.FirstOrDefault(a => a.AktenNummer == id);
            // akte.Name = input.Name;

            _dbContext.Akten.Update(new Akte() {AktenNummer = id, Name = input.Name});
            _dbContext.SaveChanges();
            return NoContent();
        }

        // DELETE https://localhost:5001/akte/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // var akte = _dbContext.Akten.FirstOrDefault(a => a.AktenNummer == id);
            // if (akte == null)
            // {
            //     return NotFound();
            // }

            _dbContext.Akten.Remove(new Akte() { AktenNummer = id});
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPut("lock/{id}")]
        [HttpPost("create-lock/{id}")]
        [HttpPost("lock/{id}")]
        public IActionResult CreateLock(string id)
        {
            return NoContent();
        }

        private bool Exists(string id) => false;
    }

    public class MvcAkte
    {
        public string Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }

    public class AkteResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class AkteInputModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
    }
}