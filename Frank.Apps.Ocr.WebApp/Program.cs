using Frank.Apps.Ocr.WebApp.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMudServices()
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    ;

builder.Logging
    .AddConsole()
    .AddDebug()
    .SetMinimumLevel(LogLevel.Debug);

builder.Services.AddHttpClient("OcrApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5201/api/");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();