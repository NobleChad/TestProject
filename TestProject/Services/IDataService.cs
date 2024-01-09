using TestProject.Models;

namespace TestProject.Services
{
    public interface IDataService<T> where T : class
    {
        T CreateItem(T item);
        void Delete(T item);
        T EditItem(T item);
        List<T> GetAllItems();
        T GetItemById(int id);
    }
}