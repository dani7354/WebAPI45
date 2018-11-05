using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var touristAttractions = _context.touristAttractions.Where(t=>t.id==id).SingleOrDefault();
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
            City city =
            _context.Cities.Where(c => c.id == cityId).SingleOrDefault();
            if (city == null)
            {
                return NotFound(city);
            }
            city.Attractions.Add(touristAttraction);
            _context.Cities.Update(city);
            _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = touristAttraction.id }, touristAttraction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
