using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;
using System.Text.Json.Serialization;

namespace JKKNIUBusBookingSystem.Pages
{
    public class DeleteScheduleModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteScheduleModel(HttpClient httpClient)
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
            if (!User.IsInRole("Admin"))
            {
                return RedirectToPage("/AvailableBuses");
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
                QueryParameters.CurrentTime = TimeSpan.Parse(DateTime.Now.ToString("HH\\:mm\\:ss"));
                QueryParameters.CurrentDate = DateOnly.Parse(DateOnly.FromDateTime(DateTime.Now).ToString("MM-dd-yyyy"));
                
                var json = JsonSerializer.Serialize(QueryParameters);
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:5134/api/schedulebus/schedulebuses", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                                        AvailableBuses = JsonSerializer.Deserialize<List<ScheduleBusDtos>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    }) ?? new List<ScheduleBusDtos>();
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
