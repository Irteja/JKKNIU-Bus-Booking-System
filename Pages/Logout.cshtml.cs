using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;


namespace JKKNIUBusBookingSystem.Pages;

    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Remove the JWT token from the cookies
            CookieHelper.RemoveJwtCookie(HttpContext);

            // Optionally, you can redirect to the login page or another page after logout
            return RedirectToPage("/Login"); // Change this if you want to redirect elsewhere
        }
    }

