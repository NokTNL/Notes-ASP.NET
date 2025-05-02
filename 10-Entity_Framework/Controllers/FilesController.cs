using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace _10_Entity_Framework.Controllers;

[Route("files")]
[ApiController]
public class FilesController : ControllerBase
{
    [HttpGet("{fileId}", Name = "GetFile")]
    public async Task<ActionResult> GetFile(string fileId)
    {
        // (...Code that looks for file that matches fileId)
        // C# relative path counts from the point of where the program starts (Program.cs, so from root location)
        var filePath = "lorem-picsum.jpg";
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }
        // Read file
        var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
        // For returning files, use the `File` method defined in ControllerBase
        // !!! Note that if the file type can change then it won't work. You may want to use `FileExtensionContentTypeProvider` instead when building the service: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.staticfiles.fileextensioncontenttypeprovider?view=aspnetcore-9.0
        return File(bytes, "image/jpeg", filePath);
    }

    // To create a file, the incoming request will have the Content-Type of `multipart/form-data`, which will be parsed into IFormFile (from ASP.NET) in our params
    // The file will be streamed
    [HttpPost]
    public ActionResult CreateFile(IFormFile file)
    {
        // Constraint the type of file that can be sent
        // - file.Length is in bytes
        if (file.Length == 0 || file.Length > 20971520 /* 20 MB */|| file.ContentType != "application/pdf")
        {
            return BadRequest("Invalid file; must not be empty or bigger than 20 MB, and must be a PDF file.");
        }
        /* ... some code for storing the file */
        return CreatedAtRoute("GetFile", new { fileId = "some-file-id"}, null);
    }
}

