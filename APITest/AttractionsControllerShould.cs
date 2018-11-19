using System;
using Xunit;
using WebAPI45;
using APITest.Mock;
using AutoMapper;
using WebAPI45.Controllers;
using WebAPI45.Model;
using System.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
namespace APITest
{
    public class AttractionsControllerShould
    {
        readonly IMapper _mapper;
        readonly TestData _testData;
        public AttractionsControllerShould()
        {
           
            _mapper = new DTOMapper().Config.CreateMapper();
            _testData = new TestData();
        }

        [Fact]
        public void PostTA()
        {
            var repo = new MockUnitOfWork();
            var controller = new AttractionsController(repo, _mapper);
            repo.Cities.AddRange(_testData.Cities);

            var attractionId = 0;
            var cityId = 1;
            var attraction = _mapper.Map<TouristAttractionDTO>(_testData.Attractions.SingleOrDefault(t => t.Id == attractionId));

            var response = controller.Post(attraction, cityId);

            var responseObjectResult = response as ObjectResult;
            Assert.Equal(201, responseObjectResult.StatusCode);
            Assert.NotNull(repo.Cities.Get(attractionId));

        }

        [Fact]
        public void GetTA()
        {
            var repo = new MockUnitOfWork();
            var controller = new AttractionsController(repo, _mapper);
            var attractionId = 2;
            repo.Cities.AddRange(_testData.Cities);
            repo.Cities.Get(0).Attractions.Add(_testData.Attractions.ElementAt(attractionId));

            var response = controller.Get(attractionId);

            var responseObjectResult = response as ObjectResult;
            Assert.Equal(200, responseObjectResult.StatusCode);
        }

        [Fact]
        public void GetAllTAs()
        {
            var repo = new MockUnitOfWork();
            var controller = new AttractionsController(repo, _mapper);
            repo.Cities.AddRange(_testData.Cities);
            repo.Cities.Get(0).Attractions.AddRange(_testData.Attractions);

            var response = controller.Get();

            var responseObjectResult = response as ObjectResult;
            Assert.Equal(200, responseObjectResult.StatusCode);

        }

        [Fact]
        public void PutTA()
        {
            var repo = new MockUnitOfWork();
            var controller = new AttractionsController(repo, _mapper);
            repo.Cities.AddRange(_testData.Cities);
            repo.Cities.Get(0).Attractions.AddRange(_testData.Attractions);
            var attractionId = 1;
            var newName = "Changed";

            var attractionDTO = _mapper.Map<TouristAttractionDTO>(_testData.Attractions.SingleOrDefault(t => t.Id == attractionId));
            attractionDTO.Name = newName;

            var response =  controller.Put(attractionId, attractionDTO);

            Assert.IsType<NoContentResult>(response);
            Assert.Equal(attractionDTO.Name, repo.TouristAttractions.Get(1).Name);



        }

        [Fact]
        public void DeleteTA()
        {
            var repo = new MockUnitOfWork();
            var controller = new AttractionsController(repo, _mapper);
            repo.Cities.AddRange(_testData.Cities);
            repo.Cities.Get(0).Attractions.AddRange(_testData.Attractions);
            var attractionId = 1;

            var response = controller.Delete(1);

            Assert.IsType<NoContentResult>(response);
            Assert.Null(repo.TouristAttractions.SingleOrDefault(t => t.Id == attractionId));


        }
    }
}