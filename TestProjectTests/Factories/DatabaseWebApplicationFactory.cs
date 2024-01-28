using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestProject.Data;
using TestProject.Services;
using TestProjectTests.TestModels;

namespace TestProjectTests.Factories
{
    public class DatabaseWebApplicationFactory : WebApplicationFactory<Program>
    {
        public readonly ItemsRepository _repository;
        public readonly ApplicationDbContext _context;
        public readonly ItemsService _service;
        public DatabaseWebApplicationFactory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _context.AddRange(ItemList.mockItems);

            _context.SaveChanges();

            _repository = new ItemsRepository(_context);
            _service = new ItemsService(_repository);
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                    options.UseInternalServiceProvider(serviceProvider);
                });
            });
        }
    }
}
