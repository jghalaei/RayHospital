using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RayHospital.Infrastructure.Data
{
    public class InMemoryData
    {
        private static Dictionary<Type, List<object>> MemoryData = new Dictionary<Type, List<object>>();
        private static InMemoryData? _instance;
        private InMemoryData()
        {

        }

        public static InMemoryData GetInstance()
        {
            if (_instance == null) _instance = new InMemoryData();
            return _instance;
        }
        public void Insert<T>(T data)
        {
            if (data == null)
                return;
            var type = typeof(T);
            if (MemoryData.ContainsKey(type))
            {
                MemoryData[type].Add(data);
            }
            else
            {
                MemoryData.Add(type, new List<object> { data });
            }
        }
        public List<T> GetAll<T>()
        {
            var type = typeof(T);
            if (MemoryData.ContainsKey(type))
            {
                return MemoryData[type].Cast<T>().ToList();
            }
            else
            {
                return new List<T>();
            }
        }
    }
}