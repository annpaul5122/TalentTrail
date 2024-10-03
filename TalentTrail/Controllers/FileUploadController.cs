using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace TalentTrail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private readonly string ApplicationName = "GoogleDriveProject";

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var credential = GoogleCredential.FromFile("C:\\ann\\hexaware\\GoogleAPI\\credentials.json")
                .CreateScoped(Scopes);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
                MimeType = file.ContentType
            };

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var request = service.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id, webContentLink";
                var fileResponse = await request.UploadAsync();

                if (fileResponse.Status != UploadStatus.Completed)
                {
                    return BadRequest("Error uploading file.");
                }

                var fileId = request.ResponseBody.Id;
                var webContentLink = request.ResponseBody.WebContentLink;

                var permission = new Google.Apis.Drive.v3.Data.Permission()
                {
                    Type = "anyone", 
                    Role = "reader"
                };

                try
                {
                    await service.Permissions.Create(permission, fileId).ExecuteAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error setting permissions: {ex.Message}");
                }

                var fileLink = $"https://drive.google.com/uc?export=view&id={fileId}";

                return Ok(new { fileId, fileLink });
            }

        }
    }
}
