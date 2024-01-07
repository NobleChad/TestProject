using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestProject.Data;
using TestProject.Services;

namespace TestProjectTests.ControllerTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<ApplicationDbContext> _context = new(new DbContextOptions<ApplicationDbContext>());
        public Mock<IProductService> _fileService = new();
    }
}
