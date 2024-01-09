using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestProject.Data;
using TestProject.Services;

namespace TestProjectTests.ControllerTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IDataRepository> _context = new(new DbContextOptions<IDataRepository>());
        public Mock<IProductService> _fileService = new();
    }
}
