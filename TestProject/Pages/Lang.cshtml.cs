using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestProject.Pages
{
    public class LangModel : PageModel
    {
        public void OnGet()
        {
            string? culture = Request.Query["culture"];
            Console.WriteLine("new selected language " + culture);
            if (culture != null) {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1)}
                    );
            }
            string returnUrl = Request.Headers["Referer"].ToString() ?? "/";
            Response.Redirect( returnUrl ); 
        }
    }
}
