var builder = WebApplication.CreateBuilder(args);

// === PONTO ÚNICO DE SEGURANÇA ===
MiniAplicacaoWeb.Modules.SecurityModule.Configure(builder);

var app = builder.Build();

// Inicializar banco de dados
MiniAplicacaoWeb.Modules.DatabaseModule.InitializeDatabase();

// Configurações de pipeline
app.UseExceptionHandler("/error"); // Tratamento de erros seguro
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery(); // Proteção CSRF

// Rota de erro genérica
app.Map("/error", () => Results.Content("Ocorreu um erro interno no servidor.", "text/html; charset=utf-8")).AllowAnonymous();

app.MapGet("/", () => Results.Redirect("/login")).AllowAnonymous();

// --- PÁGINAS SEPARADAS ---
MiniAplicacaoWeb.Endpoints.LoginEndpoints.MapLoginEndpoints(app);
MiniAplicacaoWeb.Endpoints.CadastroEndpoints.MapCadastroEndpoints(app);
MiniAplicacaoWeb.Endpoints.AdministradoresEndpoints.MapAdministradoresEndpoints(app);
MiniAplicacaoWeb.Endpoints.UsuariosEndpoints.MapUsuariosEndpoints(app);
MiniAplicacaoWeb.Endpoints.LogoutEndpoints.MapLogoutEndpoints(app);

app.Run();