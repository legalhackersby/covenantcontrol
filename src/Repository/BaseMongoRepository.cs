using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using src.Data;
using src.Models;

namespace src.Repository
{
    public abstract class BaseMongoRepository <TModel> : IBaseMongoRepository<TModel>
    where TModel: BaseEntity
    {
        private readonly IMongoDatabase mongoDatabase;

        public BaseMongoRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public TModel Get(ObjectId id)
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            var result = documents.FindSync<TModel>(model => model.Id == id).FirstOrDefault();

            return result;
        }

        public void Insert(TModel model)
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            documents.InsertOne(model);
        }

        public void InsertMany(IEnumerable<TModel> model)
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            documents.InsertMany(model);
        }

        public IEnumerable<TModel> Find(Expression<Func<TModel, bool>> filter)
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            var result = documents.FindSync<TModel>(filter).ToEnumerable();

            return result;
        }

        public IEnumerable<TModel> GetAll()
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            var result = documents.FindSync(model => true).ToEnumerable();

            return result;
        }

        public void DeleteMany(Expression<Func<TModel, bool>> filter)
        {
            var documents = mongoDatabase.GetCollection<TModel>(typeof(TModel).ToString());
            documents.DeleteMany(filter);
        }
    }
}
