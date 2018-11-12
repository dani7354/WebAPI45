using System;
using System.Collections.Generic;
using WebAPI45.Model;

namespace WebAPI45.DAL
{
    public interface ICityRepository : IRepository<City>
    {
        IEnumerable<City> GetCitiesWithTourisAttractions();
        City GetCityWithTouristAttractions(int id);
        void Update(City city);
        bool Exists(int id);
    }
}
