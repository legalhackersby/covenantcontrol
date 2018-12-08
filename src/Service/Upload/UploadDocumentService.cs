using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public class UploadDocumentService : IUploadDocumentService
    {
        public IMongoDatabase _mongoDatabase;

        public UploadDocumentService(IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public async Task<string> CreateDocument(UploadFileRequest file)
        {
            var mongoDocument = new DocumentMetadata
            {
                Id = ObjectId.GenerateNewId(),
                FileContentPath = file.Content.ToString(),
                FileContentType = file.ContentType,
                FileLength = file.Length,
                FileName = file.Name,
            };

            // autocollects collection
            var collection = _mongoDatabase.GetCollection<DocumentMetadata>("documents");

            await collection.InsertOneAsync(mongoDocument);

            return mongoDocument.Id.ToString();
        }
    }
}
