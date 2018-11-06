using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI45.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI45.Controllers
{
    [Route("api/attractions")]
    public class AttractionsController : Controller
    {
        private readonly CityDataContext _context;

        public AttractionsController(CityDataContext context)
        {
            _context = context;
        }
        // GET: api/<controller>
        [HttpGet]
        public  IEnumerable<TouristAttractions> Get()
        {
            return _context.touristAttractions;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var touristAttractions = _context.touristAttractions.FirstOrDefault(c => c.id == id);
            if (touristAttractions == null)
            {
                return NotFound(touristAttractions);
            }
            return Ok(touristAttractions);
        }

        // POST api/<controller>
        [HttpPost("{cityId}")]
        public IActionResult Post([FromBody] TouristAttractions touristAttraction, [FromRoute]int cityId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            City city = _context.Cities.Where(c => c.id == cityId).Include(c => c.Attractions).SingleOrDefault();
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
        public IActionResult Put([FromRoute]int id, [FromBody]TouristAttractions attraction)
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

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Exists(id))
            {
                return BadRequest();
            }
            var attraction = _context.touristAttractions.FirstOrDefault(t => t.id == id);
            _context.Entry(attraction).State = EntityState.Deleted;
            _context.SaveChanges();

            return Ok(attraction);
        }

        private bool Exists(int id)
        {
            return _context.touristAttractions.Any(t => t.id == id);
        }
    }
}
