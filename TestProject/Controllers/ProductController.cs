using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductService _productService;
        public ProductController(ApplicationDbContext dbContext, IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        [HttpGet]
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
            _dbContext.Add(item);
            _dbContext.SaveChanges();
            return RedirectToAction("GetAll");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var ItemFromDb = _dbContext.Items.Find(id);


            if (ItemFromDb == null)
            {
                return NotFound();
            }

            return View(ItemFromDb);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item obj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Items.Update(obj);
                _dbContext.SaveChanges();
                return RedirectToAction("GetAll");
            }
            return View(obj);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            var ItemFromDb = _dbContext.Items.Find(id);
            if (ItemFromDb == null)
            {
                return NotFound();
            }
            return View(ItemFromDb);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Item obj)
        {
            var ItemFromDb = _dbContext.Items.Find(obj.ID);
			if(ItemFromDb == null)
			{
				return NotFound();
			}
			_dbContext.Items.Remove(ItemFromDb);
            _dbContext.SaveChanges();
            return RedirectToAction("GetAll");
        }
    }
}
