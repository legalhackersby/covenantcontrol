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


        public DocumentService(IMongoDatabase mongoDatabase, IStorage storage)
        {
            this.mongoDatabase = mongoDatabase;
            this.storage = storage;
        }


        public async Task<(string, List<CovenantSearchResult>)> ReadDocument(string documentId)
        {
            var collection = this.mongoDatabase.GetCollection<DocumentMetadata>("documents");
            var id = ObjectId.Parse(documentId);
            var finder = 
                    await collection
                        .FindAsync(
                            Builders<DocumentMetadata>.Filter.Where(x => x.Id == new ObjectId(documentId))
                            );
            var singleOrDefault = await finder.SingleOrDefaultAsync();
            if (singleOrDefault == null)
            {
                return (null, null);
            } 
            var covenantsCollection = mongoDatabase.GetCollection<CovenantSearchResult>("covenants");
            var finder2 = 
                    await covenantsCollection
                        .FindAsync(
                            Builders<CovenantSearchResult>.Filter.Where(x => x.DocumentId == new ObjectId(documentId))
                            );
            var text = await storage.ReadAsync(documentId, singleOrDefault.FileNameTxt);

            return (text, finder2.ToList());
        }


    }
}
