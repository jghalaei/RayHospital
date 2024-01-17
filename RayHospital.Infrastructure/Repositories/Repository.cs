using RayHospital.Infrastructure.Data;
using RayHospital.Interfaces;

namespace RayHospital.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T>
    {

        public void Insert(T entity)
        {
            InMemoryData.GetInstance().Insert<T>(entity);
        }
        public IEnumerable<T> GetAll()
        {
            return InMemoryData.GetInstance().GetAll<T>();
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return InMemoryData.GetInstance().GetAll<T>().Where(predicate);
        }
    }
}