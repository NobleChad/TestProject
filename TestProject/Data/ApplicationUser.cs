using Microsoft.AspNetCore.Identity;

namespace TestProject.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? PFP { get; set; }
    }
}
