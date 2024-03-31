using HL7.Models;
using HL7.Services;
using Microsoft.AspNetCore.Mvc;

namespace HL7.Controllers;

[ApiController]
[Route("[controller]")]
public class HL7Controller : ControllerBase
{
    [HttpPost("upload-hl7-file")]
    public IActionResult UploadHL7File([FromBody] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        string filePath = Path.GetTempFileName();
        using (FileStream stream = System.IO.File.Create(filePath))
        {
            file.CopyTo(stream);
        }

        return Ok(new { filePath });
    }

    [HttpGet("download-hl7-file")]
    public IActionResult DownloadHL7File([FromQuery] string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return BadRequest("File path is null or empty");
        }

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found");
        }

        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", Path.GetFileName(filePath));
    }

    [HttpPost("parse-hl7-message")]
    public IActionResult ParseHL7Message([FromBody] string hl7Message)
    {
        if (string.IsNullOrEmpty(hl7Message))
        {
            return BadRequest("HL7 message is null or empty");
        }

        Message message = HL7Parser.Parse(hl7Message);
        return Ok(message);
    }
}
