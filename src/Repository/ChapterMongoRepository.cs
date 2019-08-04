using MongoDB.Driver;
using src.Models;

namespace src.Repository
{
    public class ChapterMongoRepository: BaseMongoRepository<ChapterEntity>, IChapterMongoRepository
    {
        public ChapterMongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}
