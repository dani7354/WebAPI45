using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebAPI45.DAL;
using WebAPI45.Model;

namespace APITest.Mock
{
    public class CityRepoMock : ICityRepository, ITouristAttractionRepository
    {
        private List<City> cities = new List<City>();
        public CityRepoMock()
        {
        }

        public void Add(City entity)
        {
            entity.Id = cities.Count;
            cities.Add(entity);
        }

        public void Add(TouristAttraction entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<City> entities)
        {
            cities.AddRange(entities);
        }

        public void AddRange(IEnumerable<TouristAttraction> entities)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int id)
        {
            return cities.Any(c => c.Id == id);
        }

        public IEnumerable<City> Find(Expression<Func<City, bool>> predicate)
        {
            return cities.Where(predicate.Compile());
        }

        public IEnumerable<TouristAttraction> Find(Expression<Func<TouristAttraction, bool>> predicate)
        {
            return cities.SelectMany(c => c.Attractions).Where(predicate.Compile());
        }

        public City Get(int id)
        {
            return cities.ElementAt(id);
        }

        public IEnumerable<City> GetAll()
        {
            return cities;
        }

        public IEnumerable<City> GetCitiesWithTourisAttractions()
        {
            return cities;
        }

        public City GetCityWithTouristAttractions(int id)
        {
            return Get(id);
        }

        public void Remove(City entity)
        {
            cities.Remove(entity);
        }

        public void Remove(TouristAttraction entity)
        {
            cities.FirstOrDefault(c => c.Attractions.Contains(entity)).Attractions.Remove(entity);
        }

        public void RemoveRange(IEnumerable<City> entities)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TouristAttraction> entities)
        {
            throw new NotImplementedException();
        }

        public City SingleOrDefault(Expression<Func<City, bool>> predicate)
        {
            return cities.SingleOrDefault(predicate.Compile());
        }

        public TouristAttraction SingleOrDefault(Expression<Func<TouristAttraction, bool>> predicate)
        {
            return cities.SelectMany(c => c.Attractions).SingleOrDefault(predicate.Compile());
        }

        public void Update(City city)
        {
            var oldcity = cities.FirstOrDefault(c => c.Id == city.Id);
            oldcity.Description = city.Description;
            oldcity.Name = city.Name;
            oldcity.Attractions = city.Attractions;
       
        }

        public void Update(TouristAttraction attraction)
        {

        }

        TouristAttraction IRepository<TouristAttraction>.Get(int id)
        {
            return cities.SelectMany(c => c.Attractions).FirstOrDefault(t => t.Id == id);
        }

        IEnumerable<TouristAttraction> IRepository<TouristAttraction>.GetAll()
        {
            return cities.SelectMany(c => c.Attractions);
        }





    }
}
