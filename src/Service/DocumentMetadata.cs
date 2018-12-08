using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace src.Data
{
    public class DocumentMetadata
    {
        public ObjectId Id { get; set; }

        [BsonElement("FileContentType")]
        public string FileContentType { get; set; }

        [BsonElement("FileLength")]
        public long FileLength { get; set; }

        [BsonElement("FileName")]
        public string FileName { get; set; }
    }
}
