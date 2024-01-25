using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Services
{
    public interface IDataService<T> where T : class
    {
        T CreateItem(T item);
        T? Delete(int id);
        T? EditItem(T item);
        IQueryable<T> GetAllItems(Func<DbSet<T>, IQueryable<T>> func);
		IQueryable<T> GetAllItems();
		T? GetItemById(int id);
    }
}