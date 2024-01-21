using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Services
{
    public interface IDataRepository<T> where T : class
    {
        T Create(T item);
        void Delete(T item);
        T Edit(T item);
		IQueryable<T> GetAll(Func<DbSet<T>, IQueryable<T>> func);
		IQueryable<T> GetAll();
		T GetById(int id);
    }
}