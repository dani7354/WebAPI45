using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI45.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI45.Controllers
{
    [Route("api/attractions")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        readonly CityDataContext _context;

        public AttractionsController(CityDataContext context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public  IActionResult Get()
        {
            return Ok(_context.TouristAttractions);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var touristAttractions = _context.TouristAttractions.FirstOrDefault(c => c.id == id);
            return touristAttractions == null ? NotFound(touristAttractions) : (IActionResult)Ok(touristAttractions);
        }

        // POST api/<controller>
        [HttpPost("city/{cityId}")]
        public IActionResult Post([FromBody] TouristAttraction touristAttraction, [FromRoute]int cityId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var city = _context.Cities.Where(c => c.id == cityId).Include(c => c.Attractions).SingleOrDefault();
            if (city == null)
            {
                return NotFound(city);
            }
            city.Attractions.Add(touristAttraction);
            _context.Cities.Update(city);
            _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { touristAttraction.id }, touristAttraction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]TouristAttraction attraction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != attraction.id)
            {
                return BadRequest();
            }
            _context.Entry(attraction).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAttraction([FromRoute] int id, [FromBody] JsonPatchDocument<TouristAttraction> attractionPatch)
        {
            var attraction = _context.TouristAttractions.FirstOrDefault(t => t.id == id);
            if (attraction == null)
            {
                return BadRequest(id);
            }
            attractionPatch.ApplyTo(attraction);
            _context.TouristAttractions.Update(attraction);
            _context.SaveChanges();
            return Ok(attraction);
        }


        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Exists(id))
            {
                return BadRequest(id);
            }
            var attraction = _context.TouristAttractions.FirstOrDefault(t => t.id == id);
            _context.Entry(attraction).State = EntityState.Deleted;
            _context.SaveChanges();

            return Ok(attraction);
        }

        private bool Exists(int id)
        {
            return _context.TouristAttractions.Any(t => t.id == id);
        }
    }
}
