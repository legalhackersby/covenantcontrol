using MongoDB.Bson;
using MongoDB.Driver;
using src.Models;
using System;
using System.Threading.Tasks;

namespace src.Data
{
    public class DocumentProvider : IDocumentProvider
    {
        public IMongoDatabase _mongoDatabase;

        public DocumentProvider(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public Task<Document> GetDocumentAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<string> InsertDocumentAsync(Document document)
        {
            var mongoDocument = new MongoDocument
            {
                Id = ObjectId.GenerateNewId(),

                
                FileContent = document.File.Content,
                FileContentType = document.File.ContentType,
                FileLength = document.File.Length,
                FileName = document.File.Name,
            };

            var collection = _mongoDatabase.GetCollection<MongoDocument>("documents");

            await collection.InsertOneAsync(mongoDocument);

            return mongoDocument.Id.ToString();
        }
    }
}
