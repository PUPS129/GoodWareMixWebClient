using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{
    public class Product
    {
        [BsonId]

        public string Id { get; set; }

        public string SupplierId { get; set; }

        public string InternalCode { get; set; }

        public string Title { get; set; }

        public string TitleLong { get; set; }

        public string Description { get; set; }

        public string Vendor { get; set; }

        public string VendorId { get; set; }

        public List<string> Documents { get; set; }

        public List<string> Images { get; set; }

        public string Image360 { get; set; }

        public List<string> Videos { get; set; }

        public string Categories { get; set; }

        public List<AttributeProduct> Attributes { get; set; }

        public List<Package> Packages { get; set; } 

        public List<string> Features { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool Equals(Product person)
        {

            if (this.InternalCode != person.InternalCode ||
            this.Title != person.Title ||
            this.TitleLong != person.TitleLong ||
            this.Description != person.Description ||
            this.Vendor != person.Vendor ||
            this.Image360 != person.Image360)
            {
                return false;
            }
            if ((this.Attributes == null ? 0 : this.Attributes.Count) == (person.Attributes == null ? 0 : person.Attributes.Count))
            {
                for (int i = 0; i < (this.Attributes == null ? 0 : this.Attributes.Count); i++)
                {
                    if (this.Attributes[i].Value != person.Attributes[i].Value ||
                        this.Attributes[i].Type != person.Attributes[i].Type ||
                        this.Attributes[i].Unit != person.Attributes[i].Unit ||
                        this.Attributes[i].AttributeEntity != person.Attributes[i].AttributeEntity
                        )
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            if ((this.Documents == null ? 0 : this.Documents.Count) == (person.Documents == null ? 0 : person.Documents.Count))
            {
                for (int i = 0; i < (this.Documents == null ? 0 : this.Documents.Count); i++)
                {
                    if (this.Documents[i] != person.Documents[i] 
                        )
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            if ((this.Images == null ? 0 : this.Images.Count) == (person.Images == null ? 0 : person.Images.Count))
            {
                for (int i = 0; i < (this.Images == null ? 0 : this.Images.Count); i++)
                {
                    if (this.Images[i] != person.Images[i]
                        )
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }


            if ((this.Packages == null ? 0 : this.Packages.Count) == (person.Packages == null ? 0 : person.Packages.Count))
            {
                for (int i = 0; i < (this.Packages == null ? 0 : this.Packages.Count); i++)
                {
                    if (this.Packages[i].Barcode != person.Packages[i].Barcode ||
                        this.Packages[i].Weight != person.Packages[i].Weight ||
                        this.Packages[i].Volume != person.Packages[i].Volume ||
                        this.Packages[i].PackQty != person.Packages[i].PackQty
                        )
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            if ((this.Features == null ? 0 : this.Features.Count) == (person.Features == null ? 0 : person.Features.Count))
            {
                for (int i = 0; i < (this.Features == null ? 0 : this.Features.Count); i++)
                {
                    if (this.Features[i] != person.Features[i] 
                        )
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;

        }
    }
}
