using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Service;
using System;
using System.IO;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        [HttpPost]
        public async System.Threading.Tasks.Task<string> Post(IFormFile file, [FromServices]IDocumentService documentService)
        {
            var id = Guid.NewGuid();

            byte[] content;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                content = memoryStream.ToArray();
            }

            return await documentService.CreateDocument(new Models.File
            {
                Content = content,
                ContentType = file.ContentType,
                Length = file.Length,
                Name = file.FileName,
            });
        }
    }
}
