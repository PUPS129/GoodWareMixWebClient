using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class AttributeEntity
    {
        [BsonId]
        public string Id { get; set; }
        public string SupplierId { get; set; }
        public bool Fixed { get; set; }
        public string NameAttribute { get; set; }
        public int Rating { get; set; }
        public List<string> AllValue { get; set; }
        public List<string> PossibleAttributeName { get; set; }
    }
}
