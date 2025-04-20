using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Frank.Apps.Ocr.Api.Models;
using Frank.Apps.Ocr.Api.Poppler;
using Frank.Apps.Ocr.Api.Services;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;

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
    
    [HttpPost("pdf")]
    public async Task<IActionResult> OcrPdf(
        [Required]
        [FromBody]
        FileUploadRequest? fileUploadRequest)
    {
        _logger.LogInformation("Received PDF file upload request.");
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
        
        var base64String = fileUploadRequest.Base64String.Replace("data:application/pdf;base64,", string.Empty);
        var bytes = Convert.FromBase64String(base64String);
        var fileName = fileUploadRequest.FileName;
        
        if (bytes.Length == 0)
        {
            _logger.LogError("Base64 string is empty.");
            return BadRequest("Base64 string is empty.");
        }

        _logger.LogInformation("Converting PDF to PNG pages.");
        var rawPages = await PopplerHelper.ConvertPdfToPngPagesAsync(bytes);

        var result = new OcrResponses()
        {
            FileName = fileName,
        };

        _logger.LogInformation("Performing OCR on PDF pages.");
        var pdfDocument = PdfDocument.Open(bytes, ParsingOptions.LenientParsingOff);
        
        pdfDocument.GetPages()
            .ToList()
            .ForEach(page =>
            {
                _logger.LogInformation("Page {PageNumber} of {TotalPages}", page.Number, pdfDocument.NumberOfPages);
            });
        
        result.Title = pdfDocument.Information.Title;
        result.Author = pdfDocument.Information.Author;
        result.Subject = pdfDocument.Information.Subject;
        result.Keywords = pdfDocument.Information.Keywords;
        result.Creator = pdfDocument.Information.Creator;
        result.Producer = pdfDocument.Information.Producer;
        result.CreationDate = pdfDocument.Information.CreationDate;
        
        foreach (var page in rawPages)
        {
            var ocrResult = _ocrService.PerformOcr(page.Value);
            result.Pages.Add(new OcrResponse()
            {
                PageNumber = page.Key,
                MeanConfidence = ocrResult.MeanConfidence,
                Text = ocrResult.Text,
                HOcrText = ocrResult.HOcrText,
                UnlvText = ocrResult.UnlvText,
                AltoText = ocrResult.AltoText,
                TsvText = ocrResult.TsvText,
                WordBoxes = ocrResult.WordBoxes,
                BoxText = ocrResult.BoxText,
                LstmBoxText = ocrResult.LstmBoxText
            });
        }
        
        var jsonDocument = JsonSerializer.SerializeToDocument(result);
        
        _logger.LogInformation("OCR result: {Result}", jsonDocument);
        return Ok(jsonDocument);
    }
    
}