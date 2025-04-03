using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _config = config;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GenerateJwtToken(Guid Id, string role)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing in configuration"));

        var claims = GenerateRoleBaseClaim(Id, role);

        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: creds);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        // Use IHttpContextAccessor to access HttpContext and set the cookie
        // _httpContextAccessor.HttpContext?.Response.Cookies.Append("JWToken", tokenString, new CookieOptions
        // {
        //     HttpOnly = true,
        //     Secure = true,
        //     SameSite = SameSiteMode.Strict,
        //     Expires = DateTimeOffset.Now.AddDays(10)
        // });

        return tokenString;
    }

    private Claim[] GenerateRoleBaseClaim(Guid id, string role)
    {
        return new[]
        {
            new Claim("id", id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        };
    }
}
