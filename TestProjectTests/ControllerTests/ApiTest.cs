using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using TestProject.Models;
using TestProjectTests.Factories;
using TestProjectTests.TestModels;

namespace TestProjectTests.ControllerTests
{
	public class ApiTest : IDisposable
	{
		private DataServiceWebApplicationFactory _factory;
		private HttpClient _client;
		public ApiTest()
		{
			_factory = new DataServiceWebApplicationFactory();
			_client = _factory.CreateClient();


		}
		public async Task<string> GetToken()
		{
			string email = "admin@gmail.com";
			string password = "Admin@123";
			var response = await _client.GetAsync($"https://localhost:7258/api/token?email={email}&password={password}");
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				return result;
			}
			else
			{
				return null;
			}
		}
		[Fact]
		public async Task Get_Items_Success()
		{
			//Arrange
			_factory._fileService.Setup(x => x.GetAllItems()).Returns(ItemList.mockItems.AsQueryable());
			var token = await GetToken();

			//Act
			var response = await _client.GetAsync($"/api/GetItems?t={token}");
			var result = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(ItemList.mockItems);

		}
		[Fact]
		public async Task Get_Item_By_Id_Success()
		{
			//Arrange
			var item = ItemList.mockItems.First();
			_factory._fileService.Setup(x => x.GetItemById(1)).Returns(item);
			var token = await GetToken();

			//Act
			var response = await _client.GetAsync($"/api/GetItemById/1?t={token}");
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Get_Item_By_Id_NotFound()
		{
			//Arrange
			Item item = null;
			_factory._fileService.Setup(x => x.GetItemById(1)).Returns(item);
			var token = await GetToken();

			//Act
			var response = await _client.GetAsync($"/api/GetItemById/1?t={token}");
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
		[Fact]
		public async Task Get_Filtered_Items_Success()
		{
			//Arrange
			Func<DbSet<Item>, IQueryable<Item>> func = a =>
			{
				return a.Where(b => b.Name.Length > 5);
			};
			var items = ItemList.mockItems.Where(b => b.Name.Length > 5);
			var token = await GetToken();

			_factory._fileService
				.Setup(x => x.GetAllItems(It.IsAny<Func<DbSet<Item>, IQueryable<Item>>>()))
				.Returns(items.AsQueryable());

			//Act
			var response = await _client.GetAsync($"/api/GetFilteredItems?t={token}");
			var result = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(items);
		}
		[Fact]
		public async Task Create_Item_Success()
		{
			//Arrange
			var item = new Item { ID = 4, Name = "Test4", Price = 400, Amount = 400 };
			_factory._fileService
				.Setup(x => x.CreateItem(It.Is<Item>(b => b.ID == item.ID && b.Name == item.Name && b.Price == item.Price && b.Name == item.Name)))
				.Returns(item);
			var token = await GetToken();

			//Acrt
			var response = await _client.PostAsync($"/api/CreateItem?t={token}", JsonContent.Create(item));
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Create_Item_BadRequest()
		{
			// Arrange
			Item item = null;
			var token = await GetToken();

			// Act
			var response = await _client.PostAsync($"/api/CreateItem?t={token}", JsonContent.Create(item));

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
		[Fact]
		public async Task Update_Item_Success()
		{
			//Arrange
			var item = new Item { ID = 3, Name = "Test4", Price = 239882323, Amount = 300 };
			_factory._fileService
				.Setup(x => x.EditItem(It.Is<Item>(b => b.ID == item.ID && b.Name == item.Name && b.Price == item.Price && b.Name == item.Name)))
				.Returns(item);
			var token = await GetToken();

			//Act
			var response = await _client.PutAsync($"/api/UpdateItem/3?t={token}", JsonContent.Create(item));
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Update_Item_BadRequest()
		{
			//Arrange
			Item item = null;
			var token = await GetToken();

			//Act
			var response = await _client.PutAsync($"/api/UpdateItem/123123?t={token}", JsonContent.Create(item));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
		[Fact]
		public async Task Update_Item_NotFound()
		{
			//Arrange
			Item item = new Item { ID = 123123, Name = "Test4", Price = 239882323, Amount = 300 };
			var token = await GetToken();
			//Act
			var response = await _client.PutAsync($"/api/UpdateItem/123123?t={token}", JsonContent.Create(item));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
		[Fact]
		public async Task Delete_Item_Success()
		{
			//Arrange
			_factory._fileService.Setup(x => x.Delete(1)).Returns(ItemList.mockItems.First());
			var token = await GetToken();

			//Act
			var response = await _client.DeleteAsync($"/api/DeleteItem/1?t={token}");
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
		}
		[Fact]
		public async Task Delete_Item_NotFound()
		{
			//Arrange
			var token = await GetToken();
			//Act
			var response = await _client.DeleteAsync($"/api/DeleteItem/1?t={token}");

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
		public void Dispose()
		{
			_factory.Dispose();
			_client.Dispose();
		}
	}
}
