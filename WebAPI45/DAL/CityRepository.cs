using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebAPI45.Model;
namespace WebAPI45.DAL
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityDataContext DataContext { get => Context as CityDataContext; }
        public CityRepository(CityDataContext context) : base(context) { }

        public IEnumerable<City> GetCitiesWithTourisAttractions()
        {
            return DataContext.Cities.Include(c => c.Attractions);
        }

        public City GetCityWithTouristAttractions(int id)
        {
            return DataContext.Cities.Include(c => c.Attractions).FirstOrDefault(c => c.Id == id);
        }

        public void Update(City city)
        {
            DataContext.Update(city);
        }

        public bool Exists(int id)
        {
            return DataContext.Cities.Any(c => c.Id == id);
        }
    }
}
