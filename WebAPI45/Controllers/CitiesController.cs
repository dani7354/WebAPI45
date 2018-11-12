using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        readonly CityDataContext _context;
        readonly IMapper _mapper;

        public CitiesController(CityDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Cities
        [HttpGet]
        public IActionResult GetCities([FromQuery] bool showAttractions)
        {
            return showAttractions == true
                ? Ok(_context.Cities.Include(c => c.Attractions).Select(c => _mapper.Map<CityDTOwithAttractions>(c)))
                : Ok(_context.Cities.Select(c => _mapper.Map<CityDTOnoAttractions>(c)));
        }
        // GET: api/Cities/5
        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity([FromRoute] int id, [FromQuery] bool showAttractions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = _context.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.Id == id);
            if (city == null) return BadRequest(id);
            if(showAttractions == true)
            {
                return Ok(_mapper.Map<CityDTOwithAttractions>(city));
            }
            else
            {
                return Ok(_mapper.Map<CityDTOnoAttractions>(city));
            }
        }

        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public IActionResult PutCity([FromRoute] int id, [FromBody] CityDTOwithAttractions cityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != cityDTO.Id)
            {
                return BadRequest(id);
            }

            City city = _mapper.Map<City>(cityDTO);
            using(var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Entry(city).State = EntityState.Modified;
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tran.Rollback();

                    return !CityExists(id) ? NotFound(id) : StatusCode(412, ex.Message);
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

            return CreatedAtRoute("GetCity", new { city.Id }, city);
        }
        [HttpPatch("{id}")]
        public IActionResult PatchCity([FromRoute] int id, [FromBody] JsonPatchDocument<City> cityPatch)
        {
            var city = _context.Cities.FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest(id);
            }
            cityPatch.ApplyTo(city);
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Cities.Update(city);
                    _context.SaveChanges();
                    tran.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    tran.Rollback();
                    return !CityExists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }
            }
     
            return NoContent();
        }


        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCity([FromRoute] int id)
        {
            if (!CityExists(id))
            {
                return BadRequest(id);
            }
            var city = _context.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            _context.Cities.Remove(city);
            _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
       
    }
}