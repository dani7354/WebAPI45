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
        [HttpGet("attractions={attractions:bool}")]
        public IEnumerable<City> GetCities([FromRoute] bool attractions)
        {
            if (attractions == true)
            {
                return _context.Cities.Include(c => c.Attractions);
            }
            else
            {
                return _context.Cities;
            }
        }

        // GET: api/Cities/5
        [HttpGet("{id}/attractions={attractions:bool}")]
        public IActionResult GetCity([FromRoute] int id, [FromQuery] bool attractions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            City city = null;
            if (attractions == true)
            {
                city = _context.Cities.Where(c => c.id == id).Include(c => c.Attractions).SingleOrDefault();
            }
            else
            {
                city = _context.Cities.Where(c => c.id == id).SingleOrDefault();
            }

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity([FromRoute] int id, [FromBody] City city)
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
                await _context.SaveChangesAsync();
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

            return CreatedAtAction("GetCity", new { city.id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return Ok(city);
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.id == id);
        }
    }
}