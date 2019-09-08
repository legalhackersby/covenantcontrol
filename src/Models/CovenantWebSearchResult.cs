using System;
using System.Diagnostics;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace src.Models
{
    
    //NOTE: uses same object for db and calculation to speed up dev
    [JsonObject]
    public class CovenantWebSearchResult : BaseEntity, IEquatable<CovenantWebSearchResult>
    {
        [JsonProperty]
        [BsonIgnore]
        public string CovenantId => Id.ToString();

        [BsonElement("ChapterId")]
        [JsonIgnore]
        public ObjectId ChapterId {get;set;}

        [JsonProperty]
        [BsonElement("State")]
        public CovenantState State { get; set; }

        /// <summary>
        /// Gets or sets the start covenant index.
        /// </summary>
        /// <value>
        /// The start covenant index.
        /// </value>
        [JsonProperty]
        [BsonElement("StartIndex")]
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end covenant index.
        /// </summary>
        /// <value>
        /// The end covenant index.
        /// </value>
        [JsonProperty]
        [BsonElement("EndIndex")]
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the covenant value.
        /// </summary>
        /// <value>
        /// The covenant value.
        /// </value>
        [BsonElement("CovenantValue")]
        [JsonProperty]
        public string CovenantValue { get; set; }

        /// <summary>
        /// Gets or sets the covenant value.
        /// </summary>
        /// <value>
        /// The covenant value.
        /// </value>
        [BsonElement("PageTitle")]
        [JsonProperty]
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets or sets the type of the covenant.
        /// </summary>
        /// <value>
        /// The type of the covenant.
        /// </value>
        [BsonElement("CovenantType")]
        [JsonProperty]
        public string CovenantType { get; set; }

        /// <summary>
        /// Gets or sets the covenant mathes key word.
        /// </summary>
        /// <value>
        /// The covenant mathes key word.
        /// </value>
        [JsonIgnore]
        [BsonIgnore]
        public string CovenantMathesKeyWord { get; set; }

        //https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods?noredirect=1&lq=1
        public bool IntersectNotFully(CovenantWebSearchResult other)
        {
            return this.StartIndex < other.EndIndex && other.StartIndex < this.EndIndex;
        }

        [BsonIgnore]
        public int Length
        {
            get 
            {
                Debug.Assert(CovenantValue.Length == EndIndex - StartIndex);
                return CovenantValue.Length;
            }
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StartIndex ^ EndIndex;
                return hashCode;
            }
        }

        public bool Equals(CovenantWebSearchResult other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StartIndex == other.StartIndex && EndIndex == other.EndIndex && string.Equals(CovenantValue, other.CovenantValue) && string.Equals(CovenantType, other.CovenantType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CovenantWebSearchResult) obj);
        }
    }
}
