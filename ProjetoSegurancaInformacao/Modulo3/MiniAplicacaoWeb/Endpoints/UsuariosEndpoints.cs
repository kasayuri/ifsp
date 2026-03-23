using Microsoft.AspNetCore.Antiforgery;
using MiniAplicacaoWeb.UI;

namespace MiniAplicacaoWeb.Endpoints;

public static class UsuariosEndpoints
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/usuarios", async (HttpContext ctx, IAntiforgery antiforgery) => {
            var tokenSet = antiforgery.GetAndStoreTokens(ctx);
            var html = await System.IO.File.ReadAllTextAsync("Views/usuarios.html");
            var formHtml = html.Replace("{{TokenName}}", tokenSet.FormFieldName)
                               .Replace("{{TokenValue}}", tokenSet.RequestToken);
            return Results.Content(LayoutHelper.RenderPage("Área do Usuário", formHtml), "text/html");
        }).RequireAuthorization(policy => policy.RequireRole("Comum"));
    }
}