using MongoDB.Driver;
using src.Models;

namespace src.Repository
{
    public class CovenantsWebRepository : BaseMongoRepository<CovenantWebSearchResult>, ICovenantsWebRepository
    {
        public CovenantsWebRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
        {
        }
    }
}
