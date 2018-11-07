using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI45.Model;

namespace WebAPI45.Controllers
{
    [Route("api/Cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityDataContext _context;

        public CitiesController(CityDataContext context)
        {
            _context = context;
        }
        
        // GET: api/Cities
        [HttpGet("{attractions:bool?}")]
        public IActionResult GetCities([FromRoute] bool attractions)
        {
            if (attractions == true)
            {
                return Ok(_context.Cities.Include(c => c.Attractions));
            }
            else
            {
                return Ok(_context.Cities.Select(c => new { c.id, c.name, c.description }));
            }
        }
        // GET: api/Cities/5
        [HttpGet("{id}/{attractions:bool?}", Name = "GetCity")]
        public IActionResult GetCity([FromRoute] int id, [FromRoute] bool attractions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            object city = null;
            if (attractions == true)
            {
                city = _context.Cities.Where(c => c.id == id).Include(c => c.Attractions).SingleOrDefault();
            }
            else
            {
                city = _context.Cities.Select(c => new { c.id, c.name, c.description }).FirstOrDefault(c => c.id == id);
            }
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public IActionResult PutCity([FromRoute] int id, [FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public IActionResult PostCity([FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Cities.Add(city);
            _context.SaveChangesAsync();

            return CreatedAtRoute("GetCity", new { city.id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCity([FromRoute] int id)
        {
            if (!CityExists(id))
            {
                return BadRequest(id);
            }
            var city =  _context.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.id == id);
            if (city == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(city);
             _context.SaveChangesAsync();

            return Ok(city);
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.id == id);
        }
       
    }
}