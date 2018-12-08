using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using src.Service.Upload;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public class DocumentService : IDocumentService
    {
        public IMongoDatabase mongoDatabase;
        private IStorage storage;


        public DocumentService(IMongoDatabase mongoDatabase, IStorage storage)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;

        }


        public async Task<string> ReadDocument(string documentId)
        {
            var collection = this.mongoDatabase.GetCollection<DocumentMetadata>(nameof(DocumentMetadata));
            var id = ObjectId.Parse(documentId);
            var finder = await collection.FindAsync(BuildSingleDocumentFilter(documentId));
            var singleOrDefault = await finder.SingleOrDefaultAsync();
            if (singleOrDefault == null) return null;
            return await storage.ReadAsync(documentId, singleOrDefault.FileNameTxt);
        }


        public FilterDefinition<DocumentMetadata> BuildSingleDocumentFilter(string documentId)
        {
            return Builders<DocumentMetadata>.Filter.Where(x => x.Id == new ObjectId(documentId));
        }
    }
}
