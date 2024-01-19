using RayHospital.Infrastructure.Data;
using RayHospital.Interfaces;

namespace RayHospital.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
    {
        private readonly InMemoryData _inMemoryData;
        public Repository(InMemoryData inMemoryData = null)
        {
            _inMemoryData = inMemoryData;
        }
        public void Insert(T entity)
        {
            _inMemoryData.Insert<T>(entity);
        }
        public IEnumerable<T> GetAll()
        {
            return _inMemoryData.GetAll<T>();
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _inMemoryData.GetAll<T>().Where(predicate);
        }

        public T GetOne(Func<T, bool> predicate)
        {
            return _inMemoryData.GetAll<T>().FirstOrDefault(predicate);
        }
    }
}