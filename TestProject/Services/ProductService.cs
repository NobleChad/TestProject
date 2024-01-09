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
        private readonly IDataRepository<Item> _repo;

        public ProductService(UserManager<ApplicationUser> userManager, IDataRepository<Item> repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public async Task<ItemViewModel> GetItemsAsync(ClaimsPrincipal user, int? pageNumber)
        {
            var currentUser = await _userManager.GetUserAsync(user) ?? throw new Exception("User with this role doesn't exist");
            var roles = await _userManager.GetRolesAsync(currentUser);
            IQueryable<Item> items = _repo.GetAll();
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
