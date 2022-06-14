using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class AttributeProduct
    {  
        public string AttributeEntity { get; set; }  

        public string Unit { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
