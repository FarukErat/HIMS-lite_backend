using HL7.Models;
using HL7.Services;
using Microsoft.AspNetCore.Mvc;

namespace HL7.Controllers;

[ApiController]
[Route("[controller]")]
public class HL7Controller : ControllerBase
{
    [HttpGet("parse-sample-hl7-message")]
    public IActionResult ParseSampleHL7Message()
    {
        string hl7Message = "MSH|^~\\&|SENDING_APPLICATION|SENDING_FACILITY|RECEIVING_APPLICATION|RECEIVING_FACILITY|20110613083637||ADT^A04|1817457|P|2.3|||";
        hl7Message += '\r';
        hl7Message += "EVN|A04|20110613083637|||";
        hl7Message += '\r';
        hl7Message += "PID|1||PATID1234^5^M11||JONES^WILLIAM^A^III||19610615|M-||2106-3|1200 N ELM STREET^^GREENSBORO^NC^27401-1020|GL|(555) 555-5555|(555)555-4444||S||PATID12345001^2^M10|123456789|9-87654^NC|";
        hl7Message += '\r';
        hl7Message += "NK1|1|JONES^BARBARA^K|SPO|1200 N ELM STREET^^GREENSBORO^NC^27401-1020|(555) 555-5555|(555) 555-4444|Y|";
        hl7Message += '\r';
        hl7Message += "PV1|1|I|2000^2012^01||||004777^LEBAUER^SIDNEY^J.|||SUR||||ADM|A0|";

        Message message = HL7Parser.Parse(hl7Message);
        return Ok(message);
    }

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

    [HttpPost("parse-hl7-file")]
    public IActionResult ParseHL7File([FromBody] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        string hl7Message;
        using (StreamReader reader = new(file.OpenReadStream()))
        {
            hl7Message = reader.ReadToEnd();
        }

        Message message = HL7Parser.Parse(hl7Message);
        return Ok(message);
    }
}
