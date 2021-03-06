﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI45.Model;
using WebAPI45.DAL;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI45.Controllers
{
    [Produces("application/xml", "application/json")]
    [Route("api/attractions")]
    [ApiController]
  //  [Authorize]
    public class AttractionsController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public AttractionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GET: api/<controller>
        [HttpGet("")]
        public  IActionResult Get()
        {
            var attractions = _unitOfWork.TouristAttractions.GetAll();
            var attractionsDTOs = attractions.Select(a => _mapper.Map<TouristAttractionDTO>(a));

            return Ok(attractionsDTOs);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var touristAttraction = _unitOfWork.TouristAttractions.Get(id);
            return touristAttraction == null ? NotFound(id) : (IActionResult)Ok(_mapper.Map<TouristAttractionDTO>(touristAttraction));
        }
        [HttpGet("~/api/cities/{cityId}/attractions")]
        public IActionResult GetAttractionsByCityId(int cityId)
        {
            var city = _unitOfWork.Cities.GetCityWithTouristAttractions(cityId);
            if (city == null) return NotFound(cityId);

            var attractions = city.Attractions.Select(t => _mapper.Map<TouristAttractionDTO>(t));
            if ( attractions == null) return NotFound(cityId);
            return Ok(attractions);
        }

        // POST api/<controller>
        [HttpPost("city/{cityId}")]
        public IActionResult Post([FromBody] TouristAttractionDTO touristAttraction, [FromRoute]int cityId)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var city = _unitOfWork.Cities.GetCityWithTouristAttractions(cityId);
            if (city == null)
            {
                return BadRequest(city);
            }
            TouristAttraction attraction = _mapper.Map<TouristAttraction>(touristAttraction);
            city.Attractions.Add(attraction);
            _unitOfWork.Cities.Update(city);
            _unitOfWork.Complete();

            return CreatedAtAction("Get", new { touristAttraction.Id }, touristAttraction);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put([FromRoute]int id, [FromBody]TouristAttractionDTO attractionDto)
        {
            if (!ModelState.IsValid || id != attractionDto.Id)
            {
                return BadRequest();
            }
            var touristAttraction = _mapper.Map<TouristAttraction>(attractionDto);

          
                try
                {
                _unitOfWork.TouristAttractions.Update(touristAttraction);
                _unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException ex)
                {
             
                return !_unitOfWork.TouristAttractions.Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAttraction([FromRoute] int id, [FromBody] JsonPatchDocument<TouristAttraction> attractionPatch)
        {
            var attraction = _unitOfWork.TouristAttractions.Get(id);
            if (attraction == null)
            {
                return BadRequest(id);
            }
            attractionPatch.ApplyTo(attraction);
                try
                {
                    _unitOfWork.TouristAttractions.Update(attraction);
                    _unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                return !_unitOfWork.TouristAttractions.Exists(id) ? NotFound(id) : StatusCode(412, ex.Message);
                }

            return NoContent();
        }
        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
        
            var attraction = _unitOfWork.TouristAttractions.Get(id);
            if (attraction == null) return BadRequest(id);
            _unitOfWork.TouristAttractions.Remove(attraction);
            _unitOfWork.Complete();

            return NoContent();
        }
       
    }
}
