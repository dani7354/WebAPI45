using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI45.Model
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public List<TouristAttractions> Attractions { get; set; } 


    }
}
