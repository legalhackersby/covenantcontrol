using MongoDB.Driver;
using src.Models;

namespace src.Repository
{
    public class JsonContentChangesSearchResultMongoRepository : BaseMongoRepository<JsonContentChangesSearchEntity>, IJsonContentChangesSearchResultMongoRepository
    {
        public JsonContentChangesSearchResultMongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            
        }
    }
}
