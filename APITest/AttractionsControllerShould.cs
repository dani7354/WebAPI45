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
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDTOnoAttractions, City>();
                cfg.CreateMap<CityDTOwithAttractions, City>();
                cfg.CreateMap<TouristAttractionDTO, TouristAttraction>();
            });
            _mapper = mapperConfig.CreateMapper();
            _testData = new TestData();
        }

        [Fact]
        public void PostCity()
        {

        }

        [Fact]
        public void GetCity()
        {

        }

        [Fact]
        public void GetAllCity()
        {

        }

        [Fact]
        public void PutCity()
        {

        }
        [Fact]
        public void PatchCity()
        {

        }

        [Fact]
        public void DeleteCity()
        {


        }
    }
}