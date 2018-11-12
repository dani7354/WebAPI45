using System;
namespace WebAPI45.DAL
{
    public interface IUnitOfWork
    {
        ICityRepository Cities { get; }
        ITouristAttractionRepository TouristAttractions { get; }
        int Complete();
    }
}
