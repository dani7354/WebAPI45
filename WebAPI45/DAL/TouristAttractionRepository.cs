using WebAPI45.Model;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI45.DAL
{
    internal class TouristAttractionRepository : Repository<TouristAttraction>,  ITouristAttractionRepository
    {
        public CityDataContext DataContext { get => base.Context as CityDataContext; }

        public TouristAttractionRepository(CityDataContext context) : base(context)
        {
           
        }

        public void Update(TouristAttraction attraction)
        {
            DataContext.Update(attraction);
        }

        public bool Exists(int id)
        {
            return DataContext.TouristAttractions.Any(t => t.Id == id);
        }
    }
}