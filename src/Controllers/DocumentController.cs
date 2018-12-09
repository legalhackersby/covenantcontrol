using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using src.Models;
using src.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        [HttpGet("{documentId}/covenants")]
        public async Task<object> GetCovenants(string documentId, [FromServices]IDocumentService reader)
        {
            var list = await reader.GetCovenants(documentId);

            return list.Select(x => new
            {
                id = x.CovenantId,
                type = x.CovenantType,
                description = x.CovenantValue,
                state = x.State.ToString()
            });
        }

        [HttpPost("{documentId}/covenants/{covenantId}/reject")]
        public async Task RejectCovenant(string documentId, string covenantId, [FromServices]IMongoDatabase mongoDatabase)
        {
            var covenantsCollection = mongoDatabase.GetCollection<CovenantSearchResult>("covenants");
            var filter = Builders<CovenantSearchResult>.Filter.Where(x => x.Id == new ObjectId(covenantId));
            var result = await covenantsCollection                .FindAsync(filter);

            var covenant = await result.SingleOrDefaultAsync();

            covenant.State = CovenantState.Rejected;

            await covenantsCollection.ReplaceOneAsync(filter, covenant);
        }

        [HttpPost("{documentId}/covenants/{covenantId}/accept")]
        public async Task AcceptCovenant(string documentId, string covenantId, [FromServices]IMongoDatabase mongoDatabase)
        {
            var covenantsCollection = mongoDatabase.GetCollection<CovenantSearchResult>("covenants");
            var filter = Builders<CovenantSearchResult>.Filter.Where(x => x.Id == new ObjectId(covenantId));
            var result = await covenantsCollection.FindAsync(filter);

            var covenant = await result.SingleOrDefaultAsync();

            covenant.State = CovenantState.Accepted;

            await covenantsCollection.ReplaceOneAsync(filter, covenant);
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

        private readonly string dummyCovenant = @"
      
Automating Covenants.

<mark covenantId=""0000000000000000000000000"">
Increase world social trust capatical in 2019.
</mark>

Thanks!

This is dummy contract. Upload document to see real cases.

";
    }
}
