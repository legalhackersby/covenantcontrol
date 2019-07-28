using MongoDB.Driver;
using src.Models;

namespace src.Repository
{
    public class ChapterMongoRepository: BaseMongoRepository<ChapterEntity>
    {
        public ChapterMongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}
