using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet]
        public IActionResult GetCities([FromQuery] bool showAttractions)
        {
            return showAttractions == true
                ? Ok(_context.Cities.Include(c => c.Attractions))
                : Ok(_context.Cities.Select(c => new { c.id, c.name, c.description }));
        }
        // GET: api/Cities/5
        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity([FromRoute] int id, [FromQuery] bool showAttractions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = _context.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.id == id);
            if (city == null) return BadRequest(id);
            if(showAttractions == true)
            {
                return Ok(city);
            }
            else
            {
                return Ok(_context.Cities.Select(c => new { c.id, c.name, c.description }).FirstOrDefault(c => c.id == id));
            }
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
        [HttpPatch("{id}")]
        public IActionResult PatchCity([FromRoute] int id, [FromBody] JsonPatchDocument<City> cityPatch)
        {
            var city = _context.Cities.FirstOrDefault(c => c.id == id);
            if (city == null)
            {
                return BadRequest(id);
            }
            cityPatch.ApplyTo(city);
            _context.Cities.Update(city);
            _context.SaveChanges();
            return Ok(city);
        }


        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCity([FromRoute] int id)
        {
            if (!CityExists(id))
            {
                return BadRequest(id);
            }
            var city = _context.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.id == id);
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