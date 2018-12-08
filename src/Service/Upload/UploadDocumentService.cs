using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using src.Service.Upload;
using System;
using System.Threading.Tasks;

namespace src.Service
{
    public class UploadDocumentService : IUploadDocumentService
    {
        public IMongoDatabase mongoDatabase;
        private IStorage storage;

        public UploadDocumentService(IMongoDatabase mongoDatabase, IStorage storage)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;
        }

        public async Task<string> CreateDocument(UploadFileRequest file)
        {
            var id = ObjectId.GenerateNewId();
            await storage.SaveAsync(id.ToString(), file.Content, file.Name);
            var mongoDocument = new DocumentMetadata
            {
                Id = ObjectId.GenerateNewId(),
                FileContentType = file.ContentType,
                FileLength = file.Length,
                FileName = file.Name,
            };

            // autocreates collection locally
            var collection = mongoDatabase.GetCollection<DocumentMetadata>("documents");

            await collection.InsertOneAsync(mongoDocument);

            return mongoDocument.Id.ToString();
        }
    }
}
