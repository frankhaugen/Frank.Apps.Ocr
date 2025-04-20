namespace Frank.Apps.Ocr.Api.Models;

/// <summary>
/// Represents the response from the OCR service based on a PDF file.
/// </summary>
public class OcrResponses
{
    /// File Metadata
    public string FileName { get; set; } = string.Empty;
    
    // PDF Metadata
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }
    public string? Keywords { get; set; }
    public string? Creator { get; set; }
    public string? Producer { get; set; }
    public string? CreationDate { get; set; }
    
    // OCR Metadata
    public List<OcrResponse> Pages { get; set; } = [];
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