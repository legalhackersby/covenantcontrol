using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using src.Service;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using src.Models;
using MongoDB.Bson;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        [HttpGet("{documentId}")]
        public async Task<string> Get(string documentId, [FromServices]IDocumentService reader)
        {
            if (string.IsNullOrEmpty(documentId)) throw new ArgumentException(nameof(documentId));
            var (text, covs) = await reader.ReadDocument(documentId);
            if (text != null)
            {
                var stringBuilder = new StringBuilder(text.Length);
                var head = 0;
                var ncovs = covs
                    .Where(x=> x.StartIndex < x.EndIndex)
                    .OrderBy(x=> x.StartIndex)
                    .ToList();
                var dict = new Dictionary<int, CovenantSearchResult>();
                foreach (var cov in ncovs)
                {
                    dict[cov.StartIndex] = cov;
                }

                ncovs = dict.Values.ToList();

                foreach (var cov in ncovs)
                {
                    var subs = text.Substring(head, cov.StartIndex - head);
                    stringBuilder.Append(subs);
                    stringBuilder.Append("<mark covenantId=\"" + ObjectId.GenerateNewId().ToString()  + "\">");
                    stringBuilder.Append(cov.CovenantValue);
                    stringBuilder.Append("</mark>");
                    head = cov.EndIndex;                    
                }
                stringBuilder.Append(text.Substring(head, text.Length - head));
                text = stringBuilder.ToString();
            }
            else
            {
                text = dummyCovenant;
            }
            return text.Replace(Environment.NewLine, "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
        }

        private string dummyCovenant = @"
      
Automating covenants.

<mark covenantId=""5c0bc3997ee79ebea0692de7"">
Increase world social trust captical before 2019.
</mark>

Thanks!

This is dummy covenant. Upload document to see real cases.

";
    }
}
