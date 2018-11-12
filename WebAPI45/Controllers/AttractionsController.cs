using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        readonly IMapper _mapper;

        public AttractionsController(CityDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<controller>
        [HttpGet]
        public  IActionResult Get()
        {
            return Ok(_context.TouristAttractions.Select(t => _mapper.Map<TouristAttractionDTO>(t)));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var touristAttraction = _context.TouristAttractions.FirstOrDefault(c => c.Id == id);
            return touristAttraction == null ? NotFound(id) : (IActionResult)Ok(_mapper.Map<TouristAttractionDTO>(touristAttraction));
        }

        // POST api/<controller>
        [HttpPost("city/{cityId}")]
        public IActionResult Post([FromBody] TouristAttractionDTO touristAttraction, [FromRoute]int cityId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var city = _context.Cities.Where(c => c.Id == cityId).Include(c => c.Attractions).SingleOrDefault();
            if (city == null)
            {
                return NotFound(city);
            }
            TouristAttraction attraction = _mapper.Map<TouristAttraction>(touristAttraction);
            city.Attractions.Add(attraction);
            _context.Cities.Update(city);
            _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { touristAttraction.Id }, touristAttraction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]TouristAttractionDTO attractionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != attractionDto.Id)
            {
                return BadRequest();
            }
            var touristAttraction = _mapper.Map<TouristAttraction>(attractionDto);

            using(var tran = _context.Database.BeginTransaction()){
                try
                {
                    _context.Entry(touristAttraction).State = EntityState.Modified;
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tran.Rollback();
                    return !Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }
            }
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAttraction([FromRoute] int id, [FromBody] JsonPatchDocument<TouristAttraction> attractionPatch)
        {
            var attraction = _context.TouristAttractions.FirstOrDefault(t => t.Id == id);
            if (attraction == null)
            {
                return BadRequest(id);
            }
            attractionPatch.ApplyTo(attraction);
            using(var tran = _context.Database.BeginTransaction()){
                try
                {
                    _context.Entry(attraction).State = EntityState.Modified;
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tran.Rollback();
                    return !Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }
            }
            return NoContent();
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!Exists(id))
            {
                return BadRequest(id);
            }
            var attraction = _context.TouristAttractions.FirstOrDefault(t => t.Id == id);
            _context.Entry(attraction).State = EntityState.Deleted;
            _context.SaveChanges();

            return NoContent();
        }
        private bool Exists(int id)
        {
            return _context.TouristAttractions.Any(t => t.Id == id);
        }
    }
}
