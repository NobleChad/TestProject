using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestProject.Models;
using TestProject.Services;

namespace TestProjectTests.ControllerTests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		public Mock<IDataService<Item>> _fileService { get; }
		public CustomWebApplicationFactory()
		{
			_fileService = new Mock<IDataService<Item>>();
		}
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			base.ConfigureWebHost(builder);

			builder.ConfigureTestServices(services =>
			{
				services.AddSingleton(_fileService.Object);
			});
		}
	}
}
