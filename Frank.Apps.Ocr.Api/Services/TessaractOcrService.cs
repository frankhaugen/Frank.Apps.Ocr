using Frank.Apps.Ocr.Api.Models;
using Frank.Libraries.ML.LanguageDetection;
using Microsoft.Extensions.Options;
using TesseractOCR;

namespace Frank.Apps.Ocr.Api.Services;

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