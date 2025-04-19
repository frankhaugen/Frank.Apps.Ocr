using Frank.Libraries.ML.LanguageDetection;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using TesseractOCR;
using TesseractOCR.Enums;
using Language = TesseractOCR.Enums.Language;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
return;

public class TessaractOcrService
{
    private readonly IOptions<TessaractOcrConfiguration> _tessaractOcrConfiguration;
    private readonly LanguageDetectionService _languageDetectionService = new LanguageDetectionService(new LanguageDetectionOptions());

    public TessaractOcrService(IOptions<TessaractOcrConfiguration> tessaractOcrConfiguration)
    {
        _tessaractOcrConfiguration = tessaractOcrConfiguration;
    }

    public OcrResponse PerformOcr(byte[] imageContent)
    {
        using var engine = new Engine(_tessaractOcrConfiguration.Value.TesseractDataPath, _tessaractOcrConfiguration.Value.Language, _tessaractOcrConfiguration.Value.EngineMode);
        using var img = TesseractOCR.Pix.Image.LoadFromMemory(imageContent);
        
        using var page = engine.Process(img);
        return new OcrResponse
        {
            PageNumber = page.Number,
            MeanConfidence = page.MeanConfidence,
            Text = page.Text,
            HOcrText = page.HOcrText(),
            UnlvText = page.UnlvText,
            AltoText = page.AltoText,
            TsvText = page.TsvText,
            WordBoxes = page.WordStrBoxText,
            BoxText = page.BoxText,
            LstmBoxText = page.LstmBoxText,
        };
    }
}

public class OcrResponse
{
    public int PageNumber { get; set; }
    public float MeanConfidence { get; set; }
    public string Text { get; set; }
    public string HOcrText { get; set; }
    public string UnlvText { get; set; }
    public string AltoText { get; set; }
    public string TsvText { get; set; }
    public string WordBoxes { get; set; }
    public string BoxText { get; set; }
    public string LstmBoxText { get; set; }
}

public class TessaractOcrConfiguration
{
    public string TesseractDataPath { get; set; } = "./tessdata";
    public Language Language { get; set; } = Language.English;
    public EngineMode EngineMode { get; set; } = EngineMode.Default;
}