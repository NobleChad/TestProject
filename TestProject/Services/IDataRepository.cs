using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Services
{
    public interface IDataRepository<T> where T : class
    {
        T Create(T item);
        void Delete(T item);
        T Edit(T item);
        DbSet<T> GetAll();
        T GetById(int id);
    }
}