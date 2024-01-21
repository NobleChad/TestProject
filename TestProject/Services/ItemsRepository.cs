using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Services
{
    public class ItemsRepository : IDataRepository<Item>
    {
        public ApplicationDbContext _context;
        public ItemsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

		public IQueryable<Item> GetAll()
		{
			return _context.Items;
		}
		public IQueryable<Item> GetAll(Func<DbSet<Item>, IQueryable<Item>> func)
        {
            var items = _context.Items;
            return func(items);
		}
        public Item GetById(int id)
        {
            var item = _context.Items.Find(id) ?? new Item { Name = "Item with this ID doesnt exist"};
            return item;
        }

        public Item Create(Item item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Item Edit(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(Item item)
        {
            _context.Items.Remove(item);
            _context.SaveChanges();
        }
    }
}
