namespace Frank.Apps.Ocr.WebApp.Models;

public class FileUploadRequest
{
    public string Base64String { get; set; } = null!;

    public string FileName { get; set; } = null!;
}