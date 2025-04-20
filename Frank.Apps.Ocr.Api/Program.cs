using Frank.Apps.Ocr.Api.Models;
using Frank.Apps.Ocr.Api.Poppler;
using Frank.Apps.Ocr.Api.Services;
using Scalar.AspNetCore;
using TesseractOCR.Enums;
using Language = TesseractOCR.Enums.Language;

// Initialize Poppler
await PopplerInstaller.EnsurePopplerInstalledAsync();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddHttpLogging();
builder.Services.AddSingleton<TessaractOcrService>();
builder.Services.Configure<TessaractOcrConfiguration>(options =>
{
    options.TesseractDataPath = Path.Combine(AppContext.BaseDirectory, "tessdata");
    options.Language = Language.English | Language.German | Language.French | Language.Norwegian | Language.Swedish | Language.Danish | Language.Finnish | Language.Dutch | Language.SpanishCastilian | Language.Portuguese | Language.Italian | Language.Russian | Language.Polish | Language.Czech | Language.Hungarian | Language.Slovak | Language.Slovenian | Language.Romanian | Language.Bulgarian | Language.Ukrainian | Language.GreekModern | Language.Turkish;
    options.EngineMode = EngineMode.Default;
});

builder.Logging
    .AddConsole()
    .AddDebug()
    .SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(opt =>
    {
        opt.WithTestRequestButton(true);
    });
}

app.UseHttpsRedirection();
app.UseHttpLogging();

app.MapGet("/", () => "Hello World!");
app.MapGet("/hello", () => "Hello World!");

app.MapControllers();

app.Run();
