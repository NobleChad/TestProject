using System.Security.Claims;
using TestProject.Models;

namespace TestProject.Services
{
    public interface IProductService
    {
        Task<ItemViewModel> GetItemsAsync(ClaimsPrincipal user, int? pageNumber);
    }   
}