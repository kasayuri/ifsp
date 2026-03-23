using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MiniAplicacaoWeb.Modules;
using MiniAplicacaoWeb.UI;

namespace MiniAplicacaoWeb.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this IEndpointRouteBuilder app)
    {
        // --- PÁGINA DE LOGIN (/login) ---
        app.MapGet("/login", async (HttpContext ctx, IAntiforgery antiforgery) =>
        {
            var tokenSet = antiforgery.GetAndStoreTokens(ctx);
            var html = await System.IO.File.ReadAllTextAsync("Views/login.html");
            var formHtml = html.Replace("{{TokenName}}", tokenSet.FormFieldName)
                               .Replace("{{TokenValue}}", tokenSet.RequestToken);
            return Results.Content(LayoutHelper.RenderPage("Login - Seguro App", formHtml), "text/html");
        }).AllowAnonymous();

        app.MapPost("/login", async (HttpContext ctx, [FromForm] string? login, [FromForm] string? senha) =>
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha)) 
                return Results.Redirect("/login");

            var user = DatabaseModule.GetUser(login);
            if (user != null && SecurityModule.VerifyPassword(senha, user.SenhaHash))
            {
                var claims = new[] { 
                    new Claim(ClaimTypes.Name, user.Login), 
                    new Claim(ClaimTypes.Role, user.Tipo) 
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Ao realizar o SignIn, uma nova sessão é gerada, prevenindo Session Fixation
                await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                
                if (user.Tipo == "Admin") return Results.Redirect("/administradores");
                else return Results.Redirect("/usuarios");
            }

            string errorHtml = @"
                <div class='alert alert-danger text-center mt-3' role='alert'>
                    Login ou senha inválidos. <a href='/login' class='alert-link'>Tentar novamente</a>
                </div>";
            return Results.Content(LayoutHelper.RenderPage("Erro no Login", errorHtml), "text/html; charset=utf-8");
        }).AllowAnonymous();
    }
}