using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebAPI45.Model;
namespace WebAPI45.DAL
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(CityDataContext context) : base(context) { }

        public IEnumerable<City> GetCitiesWithTourisAttractions()
        {
            return DataContext.Cities.Include(c => c.Attractions);
        }

        public CityDataContext DataContext { get => Context as CityDataContext; }

    }
}
