using Microsoft.AspNetCore.Http;
using System;

public static class CookieHelper
{
    public static void SetJwtCookie(HttpContext httpContext, string token)
    {
        httpContext.Response.Cookies.Append("JWToken", token, new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,   
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.Now.AddDays(1) 
        });
    }

    public static string? GetJwtCookie(HttpContext httpContext)
    {
        return httpContext.Request.Cookies.TryGetValue("JWToken", out var token) ? token : null;
    }
    public static void RemoveJwtCookie(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete("JWToken", new CookieOptions
        {
            HttpOnly = true, 
            Secure = true,  
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.Now.AddDays(10) 
        });
    }
}
