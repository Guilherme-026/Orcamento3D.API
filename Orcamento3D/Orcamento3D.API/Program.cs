using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

// Configura a licença gratuita do QuestPDF assim que o servidor inicia
QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles(); // Faz a API abrir o index.html automaticamente ao acessar a raiz
app.UseStaticFiles();  // Permite servir páginas HTML/CSS/JS da pasta wwwroot

app.MapControllers();

app.Run();