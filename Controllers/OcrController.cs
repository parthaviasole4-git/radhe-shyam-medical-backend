using Microsoft.AspNetCore.Mvc;
using template_backend.Models.DTOs;
using template_backend.Services;

[ApiController]
[Route("api/[controller]")]
public class OcrController : ControllerBase
{
    private readonly OcrService _ocr;

    public OcrController(OcrService ocr)
    {
        _ocr = ocr;
    }

    [HttpPost("scan")]
    public async Task<IActionResult> Scan([FromForm] IFormFile file)
    {
        var extracted = await _ocr.ExtractMedicinesAsync(file);
        var matched = await _ocr.MatchAsync(extracted);

        return Ok(matched);
    }
}
