using System;
using System.Collections.Generic;
using WebAPI45.Model;

namespace WebAPI45.DAL
{
    public interface ICityRepository
    {
        IEnumerable<City> GetCitiesWithTourisAttractions();
    }
}
