using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using MiniAplicacaoWeb.Modules;
using MiniAplicacaoWeb.UI;

namespace MiniAplicacaoWeb.Endpoints;

public static class CadastroEndpoints
{
    public static void MapCadastroEndpoints(this IEndpointRouteBuilder app)
    {
        // --- PÁGINA DE CADASTRO (/cadastro) - Somente Admin ---
        app.MapGet("/cadastro", async (HttpContext ctx, IAntiforgery antiforgery) =>
        {
            var tokenSet = antiforgery.GetAndStoreTokens(ctx);
            var html = await System.IO.File.ReadAllTextAsync("Views/cadastro.html");
            var formHtml = html.Replace("{{TokenName}}", tokenSet.FormFieldName)
                               .Replace("{{TokenValue}}", tokenSet.RequestToken);
            return Results.Content(LayoutHelper.RenderPage("Cadastro", formHtml), "text/html");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));

        app.MapPost("/cadastro", async (HttpContext ctx, [FromForm] string? login, [FromForm] string? senha, [FromForm] string? tipo) =>
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha) || (tipo != "Admin" && tipo != "Comum"))
                return Results.Content("Dados inválidos. <a href='/cadastro'>Voltar</a>", "text/html; charset=utf-8");

            // Verificar se já existe
            if (DatabaseModule.GetUser(login) != null)
                return Results.Content("Usuário já existe. <a href='/cadastro'>Voltar</a>", "text/html; charset=utf-8");

            string hash = SecurityModule.HashPassword(senha);
            DatabaseModule.CreateUser(login, hash, tipo);

            string successHtml = @"
                <div class='alert alert-success text-center mt-3' role='alert'>
                    Usuário cadastrado com sucesso! 
                    <a href='/cadastro' class='alert-link'>Novo cadastro</a> | 
                    <a href='/administradores' class='alert-link'>Voltar</a>
                </div>";
            return Results.Content(LayoutHelper.RenderPage("Sucesso", successHtml), "text/html; charset=utf-8");
        }).RequireAuthorization(policy => policy.RequireRole("Admin"));
    }
}