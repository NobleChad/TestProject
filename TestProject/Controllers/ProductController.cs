using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ProductController : Controller
    {
        private readonly IDataService<Item> _repo;
        private readonly IProductService _productService;
        public ProductController(IDataService<Item> repo, IProductService productService)
        {
            _repo = repo;
            _productService = productService;
        }

        [HttpGet]
		[Route("~/Product")]
		[Route("~/Product/GetAll{pageNumber:int:min(1)}")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<IActionResult> GetAllAsync(int? pageNumber)
        {
            var model = await _productService.GetItemsAsync(User, pageNumber);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Item item)
        {
            _repo.CreateItem(item);
            return RedirectToAction("GetAll");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var ItemFromDb = _repo.GetItemById(id);

            if (ItemFromDb == null)
            {
                return NotFound();
            }

            return View(ItemFromDb);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item)
        {
            _repo.EditItem(item);
			return RedirectToAction("GetAll");
		}
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var ItemFromDb = _repo.GetItemById(id);
            if (ItemFromDb == null)
            {
                return NotFound();
            }
            return View(ItemFromDb);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Item item)
        {
            _repo.Delete(item);
            return RedirectToAction("GetAll");
        }
    }
}
