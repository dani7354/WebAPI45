using System;
using Microsoft.EntityFrameworkCore;
using WebAPI45.Model;

namespace WebAPI45.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CityDataContext _context;
        public UnitOfWork(CityDataContext context)
        {
            _context = context;
            Cities = new CityRepository(_context);
            TouristAttractions = new TouristAttractionRepository(_context);
        }

        public ICityRepository Cities { get; }

        public ITouristAttractionRepository TouristAttractions { get; } 

       
        public int Complete()
        {   
         return _context.SaveChanges();
          
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
