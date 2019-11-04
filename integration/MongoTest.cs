using System;
using System.Collections.Generic;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Diagnostics;
using src.Models;

namespace integration
{
    public class MongoTest
    {
        public class Ping
        {
            public ObjectId Id { get; set; }
            public DateTime Pong { get; set; }
        }

        [Fact]
        public async Task Test1()
        {
            var client1 = new MongoClient();
            var testDatabase = client1.GetDatabase("Test");
            try
            {
                await testDatabase.CreateCollectionAsync(nameof(Ping));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                if (ex.CodeName == "NamespaceExists" && ex.Code == 48)
                {
                    await testDatabase.DropCollectionAsync(nameof(Ping));
                    await testDatabase.CreateCollectionAsync(nameof(Ping));
                }
                else throw;
            }

            var pings = testDatabase.GetCollection<Ping>(nameof(Ping),
                new MongoCollectionSettings {WriteConcern = WriteConcern.Acknowledged});
            await pings.InsertOneAsync(new Ping {Pong = DateTime.UtcNow});

            var client2 = new MongoClient();
            var testDatabase2 = client2.GetDatabase("Test");
            var pings2 = testDatabase2.GetCollection<Ping>(nameof(Ping),
                new MongoCollectionSettings {WriteConcern = WriteConcern.Acknowledged});
            var result = await pings2.FindAsync(FilterDefinition<Ping>.Empty);
            Assert.True(await result.AnyAsync());
        }

        [Fact]
        public async Task Test2()
        {
            var client1 = new MongoClient();
            var testDatabase = client1.GetDatabase("Test");
            try
            {
                await testDatabase.CreateCollectionAsync(nameof(TestEntity));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                if (ex.CodeName == "NamespaceExists" && ex.Code == 48)
                {
                    await testDatabase.DropCollectionAsync(nameof(TestEntity));
                    await testDatabase.CreateCollectionAsync(nameof(TestEntity));
                }
                else throw;
            }

            var pings = testDatabase.GetCollection<TestEntity>(nameof(TestEntity),
                new MongoCollectionSettings { WriteConcern = WriteConcern.Acknowledged });
            var entity = new TestEntity();
            entity.Id = ObjectId.GenerateNewId();
            entity.SubEntities = new List<TestEntity>()
            {
                new TestEntity()
                {
                    Id = ObjectId.GenerateNewId()
                },
                new TestEntity()
                {
                    Id = ObjectId.GenerateNewId()
                },
                new TestEntity()
                {
                    Id = ObjectId.GenerateNewId()
                }

            };
            await pings.InsertOneAsync(entity);

            var client2 = new MongoClient();
            var testDatabase2 = client2.GetDatabase("Test");
            var pings2 = testDatabase2.GetCollection<TestEntity>(nameof(TestEntity),
                new MongoCollectionSettings { WriteConcern = WriteConcern.Acknowledged });
            var result = await pings2.FindAsync(FilterDefinition<TestEntity>.Empty);
            Assert.True(await result.AnyAsync());
        }
    }
}

