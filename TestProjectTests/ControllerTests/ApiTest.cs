using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System.Net.Http.Json;
using TestProject.Models;
using TestProjectTests.TestModels;

namespace TestProjectTests.ControllerTests
{
	public class ApiTest : IDisposable
	{
		private CustomWebApplicationFactory _factory;
		private HttpClient _client;
		public ApiTest()
		{
			_factory = new CustomWebApplicationFactory();
			_client = _factory.CreateClient();
		}
		[Fact]
		public async Task Get_Items_Check_Success()
		{
			//Arrange
			_factory._fileService.Setup(x => x.GetAllItems()).Returns(ItemList.mockItems.AsQueryable());

			//Act
			var response = await _client.GetAsync("/api/GetItems");
			var result = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(ItemList.mockItems);

		}
		[Fact]
		public async Task Get_Item_By_Id_Check_Success()
		{
			//Arrange
			var item = ItemList.mockItems.First();
			_factory._fileService.Setup(x => x.GetItemById(1)).Returns(item);

			//Act
			var response = await _client.GetAsync("/api/GetItemById/1");
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Get_Filtered_Items_Check_Success()
		{
			//Arrange
			Func<DbSet<Item>, IQueryable<Item>> func = a =>
			{
				return a.Where(b => b.Name.Length > 5);
			};
			var items = ItemList.mockItems.Where(b => b.Name.Length > 5);

			_factory._fileService
				.Setup(x => x.GetAllItems(It.IsAny<Func<DbSet<Item>, IQueryable<Item>>>()))
				.Returns(items.AsQueryable());

			//Act
			var response = await _client.GetAsync("/api/GetFilteredItems");
			var result = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(items);
		}
		[Fact]
		public async Task Create_Item_Check_Success()
		{
			//Arrange
			var item = new Item { ID = 4, Name = "Test4", Price = 400, Amount = 400 };
			_factory._fileService
				.Setup(x => x.CreateItem(It.Is<Item>(b => b.ID == item.ID && b.Name == item.Name && b.Price == item.Price && b.Name == item.Name)))
				.Returns(item);

			//Acrt
			var response = await _client.PostAsync("/api/CreateItem", JsonContent.Create(item));
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Update_Item_Check_Success()
		{
			//Arrange
			var item = new Item { ID = 3, Name = "Test4", Price = 239882323, Amount = 300 };
			_factory._fileService
				.Setup(x => x.EditItem(It.Is<Item>(b => b.ID == item.ID && b.Name == item.Name && b.Price == item.Price && b.Name == item.Name)))
				.Returns(item);

			//Act
			var response = await _client.PutAsync("/api/UpdateItem/3", JsonContent.Create(item));
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
			result.Should().BeEquivalentTo(item);
		}
		[Fact]
		public async Task Delete_Item_Check_Success()
		{
			//Arrange
			_factory._fileService.Setup(x => x.Delete(1)).Returns(ItemList.mockItems.First());

			//Act
			var response = await _client.DeleteAsync("/api/DeleteItem/1");
			var result = JsonConvert.DeserializeObject<Item>(await response.Content.ReadAsStringAsync());

			//Assert
			response.EnsureSuccessStatusCode();
		}

		public void Dispose()
		{
			_factory.Dispose();
			_client.Dispose();
		}
	}
}
