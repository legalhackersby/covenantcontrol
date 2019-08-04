using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using src.Models;

namespace src.Repository
{
    public interface IBaseMongoRepository<TModel>
        where TModel : BaseEntity
    {
        TModel Get(ObjectId id);

        void Insert(TModel model);

        void InsertMany(IEnumerable<TModel> model);

        IEnumerable<TModel> Find(Expression<Func<TModel, bool>> filter);

        IEnumerable<TModel> GetAll();
    }
}
