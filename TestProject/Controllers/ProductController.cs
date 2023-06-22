using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult GetAll()
        {
            List<Item> items = _dbContext.Items.ToList();
            return View(items);
        }
    }
}
