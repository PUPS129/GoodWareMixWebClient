using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class Category
    {
        [BsonId]
        public string Id { get; set; }
        public string Title { get; set; }
        public string VenderId { get; set; }
        public string Description { get; set; }
        public List<Category> SubCategories { get; set; }
        //private IEnumerable<Category> GetUniqueSubcategories()
        //{
        //    return
        //        this
        //            .GetSubcategories()
        //            .ToLookup(x => x.Id)
        //            .SelectMany(xs => xs.Take(1));
        //}
        //private IEnumerable<Category> GetSubcategories()
        //{
        //    return this.SubCategories
        //        .Concat(this.SubCategories
        //            .SelectMany(x => x.GetSubcategories()));
        //}
    }
}
