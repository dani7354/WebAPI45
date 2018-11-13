using System;
using APITest.Mock;
using WebAPI45.DAL;
namespace APITest
{
    public class MockUnitOfWork : IUnitOfWork
    {
        private CityRepoMock _repoMock;
        public MockUnitOfWork()
        {
            _repoMock = new CityRepoMock();
        }

        public ICityRepository Cities => _repoMock;

        public ITouristAttractionRepository TouristAttractions => _repoMock;

        public int Complete()
        {
            return 0;
        }
    }
}
