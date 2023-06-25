using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ProductController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private string _role;
        public ProductController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        [HttpGet]
        [Route("~/Product")]
        [Route("~/Product/GetAll{pageNumber:int:min(1)}")]
        public async Task<IActionResult> GetAllAsync(int? pageNumber)
        {

            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            List<Item> items = _dbContext.Items.ToList();

            switch (roles.FirstOrDefault())
            {
                case "Admin":
                    _role = "Admin";
                    break;
                case "User":
                    _role = "User";
                    break;
                default:
                    throw new Exception("User with this role doesnt exist");
            }

            IQueryable<Item> students;
            int pageSize = 5;
            students = from s in _dbContext.Items select s;


            var model = new ItemViewModel
            {
                Role = _role,
                Paginations = await PaginatedList<Item>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize)
            };
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

        public IActionResult Delete(int? id)
        {
            var ItemFromDb = _dbContext.Items.Find(id);
            if (ItemFromDb == null)
            {
                return NotFound();
            }
            return View(ItemFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Item obj)
        {
            var ItemFromDb = _dbContext.Items.Find(obj.ID);
            _dbContext.Items.Remove(ItemFromDb);
            _dbContext.SaveChanges();
            return RedirectToAction("GetAll");
        }
        public IActionResult AboutApi()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/AboutApi.txt");
            var fileContent = System.IO.File.ReadAllText(filePath);
            return View("AboutApi", fileContent);
        }
    }
}
