using Microsoft.AspNetCore.Antiforgery;
using MiniAplicacaoWeb.UI;

namespace MiniAplicacaoWeb.Endpoints;

public static class AdministradoresEndpoints
{
    public static void MapAdministradoresEndpoints(this IEndpointRouteBuilder app)
    {
        // --- PÁGINAS RESTRITAS ---
        app.MapGet("/administradores", async (HttpContext ctx, IAntiforgery antiforgery) => {
            var tokenSet = antiforgery.GetAndStoreTokens(ctx);
            var html = await System.IO.File.ReadAllTextAsync("Views/administradores.html");
            var formHtml = html.Replace("{{TokenName}}", tokenSet.FormFieldName)
                               .Replace("{{TokenValue}}", tokenSet.RequestToken);
            return Results.Content(LayoutHelper.RenderPage("Área Admin", formHtml), "text/html");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}