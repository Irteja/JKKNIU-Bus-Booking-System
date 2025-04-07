using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JKKNIUBusBookingSystem.Pages
{
    public class SeatDetailsModel : PageModel
    {
        public Guid busScheduleId{get;set;}
        public IActionResult OnGet(Guid ScheduleId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            if (!User.IsInRole("Admin"))
            {
                return RedirectToPage("/AvailableBuses");
            }
            busScheduleId=ScheduleId;

            return Page();
        }
    }
}