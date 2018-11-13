using System;
using WebAPI45.Model;
using System.Collections.Generic;
using System.Linq;
namespace APITest.Mock
{
    public class TestData
    {
        public List<City> Cities { get; }
        public List<TouristAttraction> Attractions { get; }

        public TestData()
        {
            Cities = new List<City>()
            {
                new City()
                {
                    Id = 0,
                    Name = "Paris",
                    Description = "Hovedstad i FR",
                    Attractions = new List<TouristAttraction>()
                }, 
                new City()
                {
                    Id = 1,
                    Name = "København",
                    Description = "Hovedstad i DK",
                    Attractions = new List<TouristAttraction>()
                },
                new City()
                {
                    Id = 2,
                    Name = "Berlin",
                    Description = "Hovedstad i DE",
                    Attractions = new List<TouristAttraction>()
                }
                
            };

       

            Attractions = new List<TouristAttraction>()
            {
                new TouristAttraction()
                {
                    Id = 0,
                    Name = "Den lille havfrue",
                    Description = "Ingen"
                },
                new TouristAttraction()
                {
                    Id = 1,
                    Name = "Amalienborg Slot",
                    Description = "Et slot"
                },
                new TouristAttraction()
                {
                    Id = 2,
                    Name = "Tour Eiffel",
                    Description = "over 300 meter højt"
                }
            };
        }
    }
}
