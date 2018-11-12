using WebAPI45.Model;

namespace WebAPI45.DAL
{
    internal class TouristAttractionRepository : ITouristAttractionRepository
    {
        private CityDataContext _context;

        public TouristAttractionRepository(CityDataContext context)
        {
            _context = context;
        }
    }
}