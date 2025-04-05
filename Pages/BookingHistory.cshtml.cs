using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;

namespace JKKNIUBusBookingSystem.Pages;



public class BookingHistoryModel : PageModel
{
    public Guid UserId { get; set; }

    public IActionResult OnGet()
    {
        if (!User.Identity.IsAuthenticated || User==null)
        {
            return RedirectToPage("/login");
        }
        UserId = Guid.Parse(User.FindFirst("id").Value);
        // Console.WriteLine($"User id is {UserId}");
        return Page();
    }
}