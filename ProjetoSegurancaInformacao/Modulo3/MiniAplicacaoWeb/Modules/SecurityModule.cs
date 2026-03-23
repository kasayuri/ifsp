using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace MiniAplicacaoWeb.Modules;

public static class SecurityModule
{
    public static void Configure(WebApplicationBuilder builder)
    {
        // Controle de Acesso: Negar por padrão. Quem não tiver autorização e não tiver o AllowAnonymous é bloqueado
        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        // Mecanismo contra ataques de sessão e Cookies Seguros
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/login"; // Redireciona se a claim (papel) for insuficiente
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Força HTTPS no Cookie
                options.Cookie.SameSite = SameSiteMode.Strict; // Previne CSRF em conjunto com Antiforgery
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expira a sessão do usuário após inatividade
                options.SlidingExpiration = true;
            });

        // Prevenção CSRF Global
        builder.Services.AddAntiforgery();
    }

    // Mecanismo de Hash Seguro (BCrypt, similar ao Argon2 ou yescrypt)
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}