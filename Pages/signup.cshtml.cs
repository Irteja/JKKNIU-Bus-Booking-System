using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using JKKNIUBusBookingSystem.Dtos;

namespace JKKNIUBusBookingSystem.Pages;
public class SignupModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public SignupModel(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? RegistrationNumber { get; set; }

    [BindProperty]
    public string? RollNumber { get; set; }

    [BindProperty]
    public string? Mail { get; set; }

    [BindProperty]
    public string? Phone { get; set; }

    [BindProperty]
    public string? DepartmentName { get; set; }

    [BindProperty]
    public string? Session { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string? ConfirmPassword { get; set; }

    public string? Message { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/index");
        }
        // Any setup before the page renders
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check if passwords match
        if (Password != ConfirmPassword)
        {
            Message = "Passwords do not match.";
            return Page();
        }

        // Create a new student DTO
        var studentDto = new StudentDtos
        {
            Name = Name,
            RegistrationNumber = RegistrationNumber,
            RollNumber = RollNumber,
            Mail = Mail,
            Phone = Phone,
            DepartmentName = DepartmentName,
            Session = Session,
            Password = Password
        };

        // Send request to API for creating a student
        var content = new StringContent(JsonSerializer.Serialize(studentDto), Encoding.UTF8, "application/json");

        // Assuming your API is running locally, update this URL if needed
        var response = await _httpClient.PostAsync("http://localhost:5134/api/student/create", content);

        if (!response.IsSuccessStatusCode)
        {
            Message = "An error occurred while creating the account.";
            return Page();
        }
        var responseData = JsonSerializer.Deserialize<LoginResponseDtos>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (responseData != null)
        {
            // Store JWT token in cookies
            CookieHelper.SetJwtCookie(HttpContext, responseData.JwtToken);
        }
        // Redirect to login or some other page after successful registration
        return RedirectToPage("/login");
    }


}

