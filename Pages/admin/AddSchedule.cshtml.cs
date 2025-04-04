using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JKKNIUBusBookingSystem.Dtos;

namespace JKKNIUBusBookingSystem.Pages.Admin
{

    public class AddScheduleModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AddScheduleModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public ScheduleBusDtos NewScheduleBus { get; set; } = new();

        public List<GettingBusDtos> AvailableBuses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            if (!User.IsInRole("Admin"))
            {
                return RedirectToPage("/AvailableBuses");
            }
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("http://localhost:5134/api/bus/allbuses");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    AvailableBuses = JsonSerializer.Deserialize<List<GettingBusDtos>>(jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    ModelState.AddModelError("", "Failed to load available buses.");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error fetching bus list: " + ex.Message);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            if (!User.IsInRole("Admin"))
            {
                return RedirectToPage("/AvailableBuses");
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
                return Page();
            }

            using var client = new HttpClient();
            // Console.WriteLine($"NewScheduleBus: {JsonSerializer.Serialize(NewScheduleBus)}");
            var json = JsonSerializer.Serialize(NewScheduleBus);
            // Console.WriteLine($"Request JSON: {json}");
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5134/api/schedulebus/addschedule", content);
            Console.WriteLine($"API Response Status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/availablebuses");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                ModelState.AddModelError("", $"Failed to add schedule: {errorContent}");
                return Page();
            }
        }
    }
}
