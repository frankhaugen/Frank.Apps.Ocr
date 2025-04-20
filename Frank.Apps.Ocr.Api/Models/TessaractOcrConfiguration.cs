using TesseractOCR.Enums;

namespace Frank.Apps.Ocr.Api.Models;

public class TessaractOcrConfiguration
{
    public string TesseractDataPath { get; set; } = "./tessdata";
    public Language Language { get; set; } = Language.English;
    public EngineMode EngineMode { get; set; } = EngineMode.Default;
}