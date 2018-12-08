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
        private IConvertToTxt converter;

        public UploadDocumentService(IMongoDatabase mongoDatabase, IStorage storage, IConvertToTxt converter)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;
            this.converter = converter;
        }

        public async Task<string> CreateDocument(UploadFileRequest file)
        {
            var id = ObjectId.GenerateNewId();
            var path = await storage.SaveAsync(id.ToString(), file.Content, file.Name);
            var mongoDocument = new DocumentMetadata
            {
                Id = ObjectId.GenerateNewId(),
                FileContentType = file.ContentType,
                FileLength = file.Length,
                FileName = file.Name,
            };

            mongoDocument.FileNameTxt = await converter.ConvertAsync(path);

            // autocreates collection locally
            var collection = mongoDatabase.GetCollection<DocumentMetadata>(nameof(DocumentMetadata));

            await collection.InsertOneAsync(mongoDocument);

            return mongoDocument.Id.ToString();
        }
    }
}
