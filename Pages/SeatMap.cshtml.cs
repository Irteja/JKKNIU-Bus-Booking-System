using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;

namespace JKKNIUBusBookingSystem.Pages;

public class SeatMapModel : PageModel
{
    public Guid ScheduleId { get; set; }
    public Guid UserId{get;set;}

    public IActionResult OnGet(Guid scheduleId)
    {
        if(!User.Identity.IsAuthenticated){
            return RedirectToPage("/login");
        }
        ScheduleId = scheduleId;
        UserId=Guid.Parse(User.FindFirst("id").Value);
        return Page();
    }
}