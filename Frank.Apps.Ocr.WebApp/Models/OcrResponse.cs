namespace Frank.Apps.Ocr.WebApp.Models;

public class OcrResponse
{
    public int? PageNumber { get; set; }
    public float? MeanConfidence { get; set; }
    public string? Text { get; set; }
    public string? HOcrText { get; set; }
    public string? UnlvText { get; set; }
    public string? AltoText { get; set; }
    public string? TsvText { get; set; }
    public string? WordBoxes { get; set; }
    public string? BoxText { get; set; }
    public string? LstmBoxText { get; set; }
}