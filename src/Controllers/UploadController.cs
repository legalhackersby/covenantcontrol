using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Service;
using System;
using System.IO;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        [HttpPost]
        public async System.Threading.Tasks.Task<string> Post(IFormFile file, [FromServices]IUploadDocumentService upload)
        {
            var id = Guid.NewGuid();

            byte[] content;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                content = memoryStream.ToArray();
            }

            return await upload.CreateDocument(new Data.UploadFileRequest
            {
                Content = content,
                ContentType = file.ContentType,
                Length = file.Length,
                Name = file.FileName,
            });
        }
    }
}
