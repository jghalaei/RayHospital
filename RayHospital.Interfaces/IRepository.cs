using System;
using System.Collections.Generic;


namespace RayHospital.Interfaces;
public interface IRepository<T>
{
    public void Insert(T entity);
    public IEnumerable<T> GetAll();
    public IEnumerable<T> GetAll(Func<T, bool> predicate);
    public T GetOne(Func<T, bool> predicate);
}