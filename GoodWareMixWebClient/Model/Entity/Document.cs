using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GoodWareMixWebClient.Model
{
    public class Document
    {
        [BsonId]
        public string Id { get; set; }

        public string SupplierId { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public string CertId { get; set; }

        public string CertNumber { get; set; }

        public string CertDescr { get; set; }

        public string File { get; set; }

        public string CertOrganizNumber { get; set; }

        public string CertOrganizDescr { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string BlankNumber { get; set; }

        public bool IsDeleted { get; set; }

        public List<string> Keywords { get; set; }
    }
}