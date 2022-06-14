using MongoDB.Bson.Serialization.Attributes;

namespace GoodWareMixWebClient.Model.Entity
{
    public class Log
    {
        [BsonId]
        public string id { get; set; }
        public string SupplierId { get; set; }
        public DateTime Date { get; set; }
        public List<string> Data { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public int TotalAdded { get; set; }
        public int TotalEdit { get; set; }
        public int TotalFreezed { get; set; }
    }
}
