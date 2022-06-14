using GoodWareMixWebClient.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace GoodWareMixWebClient.Model.Entity
{
    public class SchedulerTask
    {
        [BsonId]
        public string? Id { get; set; }
        public string? NameTask { get; set; }
        public string? Description { get; set; }
        public string SupplierId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTimeOffset NextDateTask { get; set; }
        public bool IsEnable { get; set; }

        public string? CronExpression { get; set; }
    }
}
