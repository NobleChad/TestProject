using TestProject.Models;

namespace TestProject.Services
{
    public class ItemsService : IDataService<Item>
    {
        public IDataRepository<Item> _rep;
        public ItemsService(IDataRepository<Item> rep)
        {
            _rep = rep;
        }

        public List<Item> GetAllItems()
        {
            return _rep.GetAll().ToList();
        }
        public Item GetItemById(int id)
        {
            return _rep.GetById(id);
        }

        public Item CreateItem(Item item)
        {
            return _rep.Create(item);
        }

        public Item EditItem(Item item)
        {
            return _rep.Edit(item);
        }

        public void Delete(Item item)
        {
            _rep.Delete(item);
        }
    }
}
