using System;

namespace src.Models
{
    public class CovenantSearchResult : IEquatable<CovenantSearchResult>
    {
        /// <summary>
        /// Gets or sets the start covenant index.
        /// </summary>
        /// <value>
        /// The start covenant index.
        /// </value>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets the end covenant index.
        /// </summary>
        /// <value>
        /// The end covenant index.
        /// </value>
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets the covenant value.
        /// </summary>
        /// <value>
        /// The covenant value.
        /// </value>
        public string CovenantValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the covenant.
        /// </summary>
        /// <value>
        /// The type of the covenant.
        /// </value>
        public string CovenantType { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CovenantSearchResult) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = StartIndex ^ EndIndex;
                return hashCode;
            }
        }

        public bool Equals(CovenantSearchResult other)
        {
            return StartIndex == other.StartIndex && EndIndex == other.EndIndex && string.Equals(CovenantValue, other.CovenantValue) && string.Equals(CovenantType, other.CovenantType);
        }
    }
}
