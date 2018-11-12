using WebAPI45.Model;

namespace WebAPI45.DAL
{
    public interface ITouristAttractionRepository : IRepository<TouristAttraction>
    {
        void Update(TouristAttraction attraction);
        bool Exists(int id);
    }
}