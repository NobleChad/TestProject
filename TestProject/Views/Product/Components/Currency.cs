using Microsoft.AspNetCore.Mvc;

namespace TestProject.Views.Product.Components
{
	public class Currency : ViewComponent
	{
		public IViewComponentResult Invoke() {
			return View();
		}
	}
}
