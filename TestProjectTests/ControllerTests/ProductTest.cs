using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using TestProject.Controllers;
using TestProject.Data;
using TestProject.Models;

namespace TestProjectTests.ControllerTests
{
    public class ProductTest : IDisposable, IClassFixture<WebApplicationFactory<Program>>
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public ProductTest(IDataRepository context)
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task Get_Products_Returns_Ok()
        {
            // Arrange
       
            // Act
            var response = await _client.GetAsync("/Products/GetAll");

            // Assert
            response.EnsureSuccessStatusCode(); 
        }

        [Fact]
        public void Create_Item_Verify_Created()
        {
            // Arrange
            Item item = new Item()
            {
                ID = 1,
                Name = "Test",
                Price = 200,
                Amount = 5
            };
            
            ProductController controller = new ProductController(_factory._context.Object, _factory._fileService.Object);
            
            //Act
            controller.Create(item);

            // Assert
            _factory._context.Verify(c => c.Add(It.IsAny<Item>()), Times.Once);

        }

        public void Dispose()
        {
            _factory._context.Object.Dispose();
            _factory.Dispose();
            _client.Dispose();
           
        }
    }
}
