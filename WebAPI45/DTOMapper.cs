using System;
using AutoMapper;
using WebAPI45.Model;

namespace WebAPI45
{
    public class DTOMapper
    {
        public MapperConfiguration Config { get; }
        public DTOMapper()
        {
          Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDTOnoAttractions, City>();
                cfg.CreateMap<CityDTOwithAttractions, City>();
                cfg.CreateMap<TouristAttractionDTO, TouristAttraction>();
            });
        }
    }
}
