using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MiniAplicacaoWeb.Endpoints;

public static class LogoutEndpoints
{
    public static void MapLogoutEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", async (HttpContext ctx) =>
        {
            await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/login");
        }).AllowAnonymous();
    }
}