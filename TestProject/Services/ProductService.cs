using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TestProject.Data;
using TestProject.Models;

namespace TestProject.Services
{
    public class ProductService : IProductService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public ProductService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<ItemViewModel> GetItemsAsync(ClaimsPrincipal user, int? pageNumber)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            var roles = await _userManager.GetRolesAsync(currentUser);
            IQueryable<Item> items = from s in _dbContext.Items select s;
            var paginatedItems = await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, 3);

            var model = new ItemViewModel
            {
                Role = roles.FirstOrDefault() ?? throw new Exception("User with this role doesn't exist"),
                Paginations = paginatedItems
            };

            return model;
        }
    }
}
