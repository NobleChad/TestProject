using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;
using TestProjectTests.Factories;

namespace TestProjectTests.ServicesTests
{
    public class ItemsRepositoryTests : IDisposable, IClassFixture<DatabaseWebApplicationFactory>
    {
        private readonly DatabaseWebApplicationFactory _factory;

        public ItemsRepositoryTests(DatabaseWebApplicationFactory factory)
        {
            _factory = factory;

        }
        [Fact]
        public void GetAll_ReturnsAllItems()
        {
            // Act
            var items = _factory._repository.GetAll().AsNoTracking().ToList();

            // Assert
            Assert.Equal(_factory._context.Items.ToList().Count, items.Count);
        }
        [Fact]
        public void GetAllFiltered_ReturnsAllItems()
        {
            //Arrange
            Func<DbSet<Item>, IQueryable<Item>> func = a =>
            {
                return a.Where(b => b.Name.Length > 5);
            };
            // Act
            var items = _factory._repository.GetAll(func).ToList();

            // Assert
            Assert.Equal(_factory._context.Items.Where(b => b.Name.Length > 5).AsNoTracking().ToList().Count, items.Count);
        }
        [Fact]
        public void GetItemById_ReturnsOneItem()
        {
            //Assert
            int id = 1;
            // Act
            var item = _factory._repository.GetById(id);
            var existingItem = _factory._context.Items.Find(id);
            // Assert
            item.Should().BeEquivalentTo(existingItem);
        }
        [Fact]
        public void CreateItem_ReturnsItem()
        {
            var item = new Item { ID = 4, Name = "Test4", Price = 400, Amount = 400 };
            // Act
            var newItem = _factory._repository.Create(item);
            var existingItem = _factory._context.Items.Find(item.ID);

            // Assert
            newItem.Should().BeEquivalentTo(existingItem);
        }
        [Fact]
        public void UpdateItem_ReturnsItem()
        {
            var item = new Item { ID = 3, Name = "Test4", Price = 400, Amount = 400 };
            // Act
            var newItem = _factory._repository.Edit(item);
            var existingItem = _factory._context.Items.Find(item.ID);

            // Assert
            newItem.Should().BeEquivalentTo(existingItem);

        }
        [Fact]
        public void Delete_ReturnsItem()
        {
            int id = 1;
            // Act
            _factory._repository.Delete(id);

            // Assert
            Assert.Null(_factory._context.Items.Find(id));
        }
        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}