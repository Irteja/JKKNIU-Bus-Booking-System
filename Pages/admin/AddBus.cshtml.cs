using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JKKNIUBusBookingSystem.Pages
{
    public class AddBusModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/login");
            }
            return Page();
        }
    }
}