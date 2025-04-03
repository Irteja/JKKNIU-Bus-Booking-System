using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JKKNIUBusBookingSystem.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JKKNIUBusBookingSystem.Pages;
public class LoginModel : PageModel
{
    private readonly HttpClient _httpClient;

    public LoginModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public IActionResult OnGet()
    {
        // Check if the user is already authenticated
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/index");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ModelState.AddModelError("", "Email and Password are required.");
            return Page();
        }

        var loginData = new { Mail = Email, Password = Password };
        var content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("http://localhost:5134/api/student/login", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return Page();
        }

        var responseData = JsonSerializer.Deserialize<LoginResponseDtos>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (responseData != null)
        {
            // Store JWT token in cookies
            CookieHelper.SetJwtCookie(HttpContext, responseData.JwtToken);

            return RedirectToPage("/privacy");
        }

        ModelState.AddModelError("", "Something went wrong.");
        return Page();
    }

}
