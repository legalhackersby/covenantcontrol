using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using src.Service;
using System.Text;
using System.Collections.Generic;
using src.Models;
using System.Linq;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        [HttpGet("{documentId}/covenants")]
        public async Task<object> GetCovenants(string documentId, [FromServices]IDocumentService reader)
        {
            var list = await reader.GetCovenants(documentId);

            return list.Select(x => new {
                id = x.CovenantId,
                type = x.CovenantType,
                description = x.CovenantValue,
                state = x.State.ToString()
            });
        }

        [HttpGet("{documentId}")]
        public async Task<string> Get(string documentId, [FromServices]IDocumentService reader)
        {
            if (string.IsNullOrEmpty(documentId)) throw new ArgumentException(nameof(documentId));
            var (text, covs) = await reader.ReadDocument(documentId);
            if (text != null)
            {
                var stringBuilder = new StringBuilder(text.Length);
                var head = 0;
                List<CovenantSearchResult> ncovs = covs;

                foreach (var cov in ncovs)
                {
                    var subs = text.Substring(head, cov.StartIndex - head);
                    stringBuilder.Append(subs);
                    stringBuilder.Append("<mark id=\"" + cov.CovenantId + "\">");
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
      
Automating Covenants.

<mark covenantId=""0000000000000000000000000"">
Increase world social trust capatical in 2019.
</mark>

Thanks!

This is dummy contract. Upload document to see real cases.

";
    }
}
