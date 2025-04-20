using System.ComponentModel.DataAnnotations;

namespace Frank.Apps.Ocr.Api.Models;

public class FileUploadRequest
{
    [Required]
    public string Base64String { get; set; } = null!;
    
    [Required]
    public string FileName { get; set; } = null!;
}