using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using src.Models;
using src.Service.Document;
using src.Service.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Service
{
    public class UploadDocumentService : IUploadDocumentService
    {
        public IMongoDatabase mongoDatabase;
        private IStorage storage;
        private IConvertToTxt converter;
        private ITextParserService convenantsSearch;

        public UploadDocumentService(IMongoDatabase mongoDatabase, IStorage storage, IConvertToTxt converter, ITextParserService convenantsSearch)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;
            this.converter = converter;
            this.convenantsSearch = convenantsSearch;
        }

        public async Task<string> CreateDocument(UploadFileRequest file)
        {
            var id = ObjectId.GenerateNewId();
            var path = await storage.SaveAsync(id.ToString(), file.Content, file.Name);
            var mongoDocument = new DocumentMetadata
            {
                Id = id,
                FileContentType = file.ContentType,
                FileLength = file.Length,
                FileName = file.Name,
            };

            mongoDocument.FileNameTxt = await converter.ConvertAsync(path);

            var content = await storage.ReadAsync(id.ToString(), mongoDocument.FileNameTxt);

            var covenants = GetValidCovenants(convenantsSearch.GetCovenantResults(content));
            foreach (var cov in covenants)
            {
                cov.Id = ObjectId.GenerateNewId();
                cov.DocumentId = id;
            }

            // autocreates collection locally
            var documents = mongoDatabase.GetCollection<DocumentMetadata>("documents");
            var covenantsCollection = mongoDatabase.GetCollection<CovenantSearchResult>("covenants");

            await documents.InsertOneAsync(mongoDocument);
            await covenantsCollection.InsertManyAsync(covenants);
            
            return mongoDocument.Id.ToString();
        }

        private static List<CovenantSearchResult> GetValidCovenants(List<CovenantSearchResult> covs)
        {
            // ISSUE: not optimal at all, fix on search engine side
            var ncovs = covs
                .Where(x => x.StartIndex < x.EndIndex)//BUG: will be ensured by tests
                .OrderBy(x => x.StartIndex)
                .ToList();
            var dict = new Dictionary<int, CovenantSearchResult>();
            foreach (var cov in ncovs)
            {
                dict[cov.StartIndex] = cov;// BUG: we just use one covenant, but should all
            }

            var result = new List<CovenantSearchResult>();
               ncovs = dict.Values.ToList();
            foreach (var n in ncovs)
            {
                if (!result.Any(n.IntersectNotFully)) result.Add(n);
            }

         
            return result;
        }
    }
}
