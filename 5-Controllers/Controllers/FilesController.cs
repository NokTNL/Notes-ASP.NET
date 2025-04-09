using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace _5_Controllers.Controllers;

[Route("files")]
[ApiController]
public class FilesController : ControllerBase
{
    [HttpGet]
    [Route("{fileId}")]
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
}

