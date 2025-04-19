using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Frank.Apps.Ocr.Api.Controlles;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class OcrController : ControllerBase
{
    private readonly ILogger<OcrController> _logger;
    private readonly TessaractOcrService _ocrService;

    public OcrController(ILogger<OcrController> logger, TessaractOcrService ocrService)
    {
        _logger = logger;
        _ocrService = ocrService;
    }

    [HttpPost]
    public async Task<IActionResult> Ocr(
        [Required]
        [FromBody]
        FileUploadRequest? fileUploadRequest)
    {
        if (fileUploadRequest == null)
        {
            _logger.LogError("File upload request is null.");
            return BadRequest("File upload request is null.");
        }
        
        if (string.IsNullOrEmpty(fileUploadRequest.Base64String))
        {
            _logger.LogError("Base64 string is null or empty.");
            return BadRequest("Base64 string is null or empty.");
        }
        
        if (string.IsNullOrEmpty(fileUploadRequest.FileName))
        {
            _logger.LogError("File name is null or empty.");
            return BadRequest("File name is null or empty.");
        }
        
        var base64String = fileUploadRequest.Base64String.Replace("data:image/png;base64,", string.Empty);
        var bytes = Convert.FromBase64String(base64String);
        var fileName = fileUploadRequest.FileName;
        
        if (bytes.Length == 0)
        {
            _logger.LogError("Base64 string is empty.");
            return BadRequest("Base64 string is empty.");
        }
        using var fileContent = new MemoryStream(bytes);

        using var stream = new MemoryStream();
        await fileContent.CopyToAsync(stream);
        stream.Position = 0;

        var result = _ocrService.PerformOcr(stream.ToArray());
        
        _logger.LogInformation("OCR result: {Result}", result);
        
        if (string.IsNullOrEmpty(result.Text))
        {
            _logger.LogError("OCR failed for file: {FileName}", fileName);
            return BadRequest("OCR failed.");
        }
        
        var jsonDocument = JsonSerializer.SerializeToDocument(result);

        return Ok(jsonDocument);
    }
    
}

public class FileUploadRequest
{
    [Required]
    public string Base64String { get; set; } = null!;
    
    [Required]
    public string FileName { get; set; } = null!;
}