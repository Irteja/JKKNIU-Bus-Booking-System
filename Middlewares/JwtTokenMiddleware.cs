using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the cookie contains a JWT token
        if (context.Request.Cookies.TryGetValue("JWToken", out var token) && !string.IsNullOrEmpty(token))
        {
            // Add the JWT token to the Authorization header
            context.Request.Headers["Authorization"] = $"Bearer {token}";
        }

        await _next(context);
    }
}

// Extension method for easy middleware registration
public static class JwtTokenMiddlewareExtensions
{
    public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<JwtTokenMiddleware>();
    }
}
