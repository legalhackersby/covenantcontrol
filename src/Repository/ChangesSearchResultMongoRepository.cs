using MongoDB.Driver;
using src.Models;

namespace src.Repository
{
    public class ChangesSearchResultMongoRepository : BaseMongoRepository<ChangesSearchEntity>, IChangesSearchResultMongoRepository
    {
        public ChangesSearchResultMongoRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
            
        }
    }
}
