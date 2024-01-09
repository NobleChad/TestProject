using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Services
{
    public class ItemsRepository : IDataRepository<Item>
    {
        public IDataRepository _context;
        public ItemsRepository(IDataRepository context)
        {
            _context = context;
        }

        public DbSet<Item> GetAll()
        {
            var items = _context.Items;
            return items;
        }
        public Item GetById(int id)
        {
            var item = _context.Items.Find(id) ?? throw new Exception("The item doesn't exist");
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
