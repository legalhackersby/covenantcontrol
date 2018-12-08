using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using src.Models;
using src.Service.Document;
using src.Service.Upload;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace src.Service
{
    public class DocumentService : IDocumentService
    {
        public IMongoDatabase mongoDatabase;
        private IStorage storage;
        private ITextParserService covenantsSearch;

        public DocumentService(IMongoDatabase mongoDatabase, IStorage storage, ITextParserService covenanstSearch)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;
            this.covenantsSearch = covenanstSearch;

        }


        public async Task<(string, List<CovenantSearchResult>)> ReadDocument(string documentId)
        {
            var collection = this.mongoDatabase.GetCollection<DocumentMetadata>("documents");
            var id = ObjectId.Parse(documentId);
            var finder = await collection.FindAsync(BuildSingleDocumentFilter(documentId));
            var singleOrDefault = await finder.SingleOrDefaultAsync();
            if (singleOrDefault == null)
            {
                return (null, null);
            } 


            var text = await storage.ReadAsync(documentId, singleOrDefault.FileNameTxt);
            var covenants = covenantsSearch.GetCovenantResults(text);
            return (text, covenants);
        }


        public FilterDefinition<DocumentMetadata> BuildSingleDocumentFilter(string documentId)
        {
            return Builders<DocumentMetadata>.Filter.Where(x => x.Id == new ObjectId(documentId));
        }
    }
}
