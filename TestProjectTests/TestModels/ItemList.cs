using TestProject.Models;

namespace TestProjectTests.TestModels
{
	public static class ItemList
	{
		public static readonly List<Item> mockItems = new(){
		new Item { ID = 1,Name="Test1", Price = 100, Amount = 100},
		new Item { ID = 2,Name="Test2sadasdsa", Price = 200, Amount = 200},
		new Item { ID = 3,Name="Test3sadasdads", Price = 300, Amount = 300}
		};

	}
}
