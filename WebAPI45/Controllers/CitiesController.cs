using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI45.DAL;
using WebAPI45.Model;
using WebAPI45;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI45.Controllers
{
    [Produces("application/xml", "application/json")]
    [Route("api/Cities")]
    [ApiController]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public CitiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Cities
      
        [HttpGet("")]
        public IActionResult GetCities([FromQuery] bool showAttractions)
        {
            return showAttractions == true
                ? Ok(_unitOfWork.Cities.GetCitiesWithTourisAttractions().Select(c => _mapper.Map<CityDTOwithAttractions>(c)))
                    : Ok(_unitOfWork.Cities.GetCitiesWithTourisAttractions().Select(c => _mapper.Map<CityDTOnoAttractions>(c)));
        }
        // GET: api/Cities/5
        [HttpGet("{id}", Name = "GetCity")]
        public IActionResult GetCity([FromRoute] int id, [FromQuery] bool showAttractions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            City city = _unitOfWork.Cities.GetCityWithTouristAttractions(id);
            if (city == null) return BadRequest(id);
            if (showAttractions == true)
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
                try
                {
                _unitOfWork.Cities.Update(city);
                _unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException ex)
                {

                return !_unitOfWork.Cities.Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
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

            _unitOfWork.Cities.Add(city);
            _unitOfWork.Complete();

            return CreatedAtRoute("GetCity", new { city.Id }, city);
        }
        [HttpPatch("{id}")]
        public IActionResult PatchCity([FromRoute] int id, [FromBody] JsonPatchDocument<City> cityPatch)
        {
            var city = _unitOfWork.Cities.SingleOrDefault(c => c.Id == id);
            if (city == null)
            {
                return BadRequest(id);
            }
            cityPatch.ApplyTo(city);
       
                try
                {
                _unitOfWork.Cities.Update(city);
                _unitOfWork.Complete();
               
                }
                catch (DbUpdateConcurrencyException ex)
                {
                return !_unitOfWork.Cities.Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }
            return NoContent();
        }


        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCity([FromRoute] int id)
        {
            if (!_unitOfWork.Cities.Exists(id))
            {
                return BadRequest(id);
            }
            var city = _unitOfWork.Cities.GetCityWithTouristAttractions(id);
            if (city == null)
            {
                return NotFound();
            }
            _unitOfWork.Cities.Remove(city);
            _unitOfWork.Complete();

            return NoContent();
        }
    }
}