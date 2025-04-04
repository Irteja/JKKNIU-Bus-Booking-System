using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;

namespace JKKNIUBusBookingSystem.Pages
{
    public class AvailableBusesModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public AvailableBusesModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<ScheduleBusDtos> AvailableBuses { get; set; } = new();

        [BindProperty]
        public QueryParameterForGettingScheduleBus QueryParameters { get; set; } = new QueryParameterForGettingScheduleBus();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using var client = new HttpClient();
                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                    }
                    return Page();
                }

                // Normalize TimeSpan to include seconds if missing
                QueryParameters.CurrentTime = TimeSpan.Parse(DateTime.Now.ToString("HH\\:mm\\:ss"));
                QueryParameters.CurrentDate = DateOnly.Parse(DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy"));
                Console.WriteLine("Current Time ->" + QueryParameters.CurrentTime);
                var json = JsonSerializer.Serialize(QueryParameters);
                Console.WriteLine("Request JSON: " + json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5134/api/schedulebus/schedulebuses", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    AvailableBuses = JsonSerializer.Deserialize<List<ScheduleBusDtos>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ScheduleBusDtos>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
                    ModelState.AddModelError("", "Failed to load available schedules.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error fetching schedule: " + ex.Message);
            }

            return Page();
        }
    }
}
