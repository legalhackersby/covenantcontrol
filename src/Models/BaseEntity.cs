using System;
using MongoDB.Bson;

namespace src.Models
{
    public class BaseEntity
    {
        public ObjectId Id { get; set; }
    }
}
