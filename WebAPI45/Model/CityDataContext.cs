using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI45.Model
{
    public class CityDataContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<TouristAttractions> touristAttractions { get; set; }
        public CityDataContext(DbContextOptions<CityDataContext>options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}
