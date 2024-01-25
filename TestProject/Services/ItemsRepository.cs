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
        public Item? GetById(int id)
        {
            var item = _context.Items.Find(id);
            return item;
        }

        public Item Create(Item item)
        {
            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Item? Edit(Item item)
        {
			var existingItem = _context.Items.Local.FirstOrDefault(e => e.ID == item.ID);
			if (existingItem != null)
			{
				_context.Entry(existingItem).State = EntityState.Detached;
			}

			var result = _context.Items.Find(item.ID);
			if (result == null)
			{
				return null;
			}

			_context.Entry(result).CurrentValues.SetValues(item);

			_context.SaveChanges();
			return item;
		}

        public Item? Delete(int id)
        {
            var result = _context.Items.Find(id);
			if (result == null) {
                return null;
			}
			_context.Items.Remove(result);
			_context.SaveChanges();
			return result;
		}
    }
}
