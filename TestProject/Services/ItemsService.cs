using Microsoft.EntityFrameworkCore;
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
		public IQueryable<Item> GetAllItems()
		{
			return _rep.GetAll();
		}
		///<summary> 
		///method to get all items
		///</summary>
		public IQueryable<Item> GetAllItems(Func<DbSet<Item>, IQueryable<Item>> func)
        {
            return _rep.GetAll(func);
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
