using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;
using TestProject.Data;
using TestProject.Services;
using TestProjectTests.Factories;
using TestProjectTests.TestModels;

namespace TestProjectTests.ServicesTests
{
	public class ProductServiceTest : IDisposable
	{
		private DataServiceWebApplicationFactory _factory;
		private Mock<UserManager<ApplicationUser>> _userManager;
		private ProductService _service;
		public ProductServiceTest()
		{
			_factory = new DataServiceWebApplicationFactory();
			var userStore = new Mock<IUserStore<ApplicationUser>>();
			_userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
			_service = new ProductService(_userManager.Object, _factory._fileService.Object);
		}

		[Fact]
		public async Task GetItemsAsync_Success()
		{
			//Arrange
			var claim = new ClaimsPrincipal();
			var user = new ApplicationUser() { Name = "gamich", PFP = null };
			var role = new List<string> { "Admin" };
			var items = ItemList.mockItems.AsQueryable().BuildMock();

			_userManager.Setup(x => x.GetUserAsync(claim)).ReturnsAsync(user);
			_userManager.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(role);

			//Act
			_factory._fileService.Setup(x => x.GetAllItems()).Returns(items);
			var result = await _service.GetItemsAsync(claim, 1);

			//Assert
			result.Paginations.Should().BeEquivalentTo(ItemList.mockItems);
		}

		public void Dispose()
		{
			_factory.Dispose();
		}
	}
}
