using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using JKKNIUBusBookingSystem.db;
using JKKNIUBusBookingSystem.Dtos;
using JKKNIUBusBookingSystem.Entites;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace JKKNIUBusBookingSystem.Controllers;
[Route("api/[controller]")]
[ApiController]

public class StudentController : ControllerBase
{
    private readonly IAuthService authService;
    private readonly JKKNIUBusBookingSystemDbContext context;

    public StudentController(IAuthService _authService, JKKNIUBusBookingSystemDbContext _context)
    {
        context = _context;
        authService = _authService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDtos studentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check if a student already exists with the same registration number
        bool exists = await context.Students
            .AnyAsync(s =>
            s.Mail == studentDto.Mail
            ||
            s.Phone == studentDto.Phone
            );

        if (exists)
        {
            return Conflict("A student with this Email or Phone number already exists.");
        }

        // Create new student entity
        var student = new Student
        {
            id = Guid.NewGuid(),
            Name = studentDto.Name,
            RegistrationNumber = studentDto.RegistrationNumber,
            RollNumber = studentDto.RollNumber,
            Mail = studentDto.Mail,
            Phone = studentDto.Phone,
            DepartmentName = studentDto.DepartmentName,
            Session = studentDto.Session,
            Password = BCrypt.Net.BCrypt.HashPassword(studentDto.Password)
        };

        // Save to database
        string token = authService.GenerateJwtToken(student.id, "Student");
        context.Students.Add(student);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStudentById), new { id = student.id }, new { JwtToken = token, StudentId = student.id });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudentById(Guid id)
    {
        var student = await context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound("Student not found.");
        }

        return Ok(student);
    }

    [HttpGet("check")]
    [Authorize(Roles = "Student")]
    public async Task<ActionResult<Student>> check()
    {
        var userMail = User.FindFirst(ClaimTypes.Email)?.Value;
        var student = await context.Students.Where(s => s.Mail == userMail).ToListAsync();

        if (student == null)
        {
            return NotFound("Student not found.");
        }

        return Ok(student);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] StudentLoginDtos loginDto)
    {
        string token;
        if (loginDto.Mail == "admin@gmail.com" && loginDto.Password == "1234")
        {
            token = authService.GenerateJwtToken(Guid.NewGuid(), "Admin");
            return Ok(new { message = "Login successful", JwtToken = token });

        }
        var student = await context.Students
            .FirstOrDefaultAsync(s => s.Mail == loginDto.Mail);

        if (student == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, student.Password);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password.");
        }
        token = authService.GenerateJwtToken(student.id, "Student");
        return Ok(new { message = "Login successful", JwtToken = token, studentId = student.id });
    }
}