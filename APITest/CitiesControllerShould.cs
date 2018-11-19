using System;
using Xunit;
using WebAPI45.Controllers;
using APITest.Mock;
using AutoMapper;
using WebAPI45.Model;
using WebAPI45;
using System.Linq;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace APITest
{
    public class CitiesControllerShould
    {
        readonly IMapper _mapper;
        readonly TestData _testData;

        public CitiesControllerShould()
        {
           
            _mapper = new DTOMapper().Config.CreateMapper();
            _testData = new TestData();

        }

        [Fact]
        public void PostCity()
        {
            //Arrange
            var repo = new MockUnitOfWork();
            var controller = new CitiesController(repo, _mapper);
            var newCity = _testData.Cities.First();

            //Act
            var response = controller.PostCity(newCity);

            //Assert
            var responseObjectResult = response as ObjectResult;
            Assert.Equal(201, responseObjectResult.StatusCode);
            Assert.NotNull(repo.Cities.Get(newCity.Id));
        }

        [Fact]
        public void GetCity()
        {
            //Arrange
            var repo = new MockUnitOfWork();
            var controller = new CitiesController(repo, _mapper);
            var city1 = _testData.Cities.First();
            var city2 = _testData.Cities.Last();
            controller.PostCity(city1);
            controller.PostCity(city2);

            //Act
            var response = controller.GetCity(city2.Id, false);

            // Assert
            var responseObjectResult = response as ObjectResult;
            Assert.Equal(200, responseObjectResult.StatusCode);
        }

        [Fact]
        public void GetAllCities()
        {
            var repo = new MockUnitOfWork();
            var controller = new CitiesController(repo, _mapper);
            repo.Cities.AddRange(_testData.Cities);
        

            var response = controller.GetCities(false);

            var responseObjectResult = response as ObjectResult;
            Assert.Equal(200, responseObjectResult.StatusCode);
        }

        [Fact]
        public void PutCity()
        {
            var repo = new MockUnitOfWork();
            var controller = new CitiesController(repo, _mapper);
            repo.Cities.Add(_testData.Cities.First());
            var city = new CityDTOwithAttractions() { Id = 0, Name = "Changed", Description = "Hovedstad i FR", Attractions = new List<TouristAttraction>(){_testData.Attractions.SingleOrDefault(t => t.Id == 2)}  };

            var response = controller.PutCity(0, city);

        
            Assert.IsType<NoContentResult>(response);
            Assert.Equal(city.Name, repo.Cities.Get(0).Name);

        }

        [Fact]
        public void DeleteCity()
        {
            var repo = new MockUnitOfWork();
            var controller = new CitiesController(repo, _mapper);
            var citiyId = 0;
            repo.Cities.Add(_testData.Cities.FirstOrDefault(c => c.Id == citiyId));

            var response = controller.DeleteCity(citiyId);

            Assert.IsType<NoContentResult>(response);
            Assert.Null(repo.Cities.SingleOrDefault(c => c.Id == citiyId));

        }
    }
}
