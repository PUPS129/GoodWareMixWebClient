using GoodWareMixWebClient.Model.Entity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodWareMixWebClient.Model
{

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ProFileSupplier
    {
        [BsonId]
        public string? Id { get; set; }

        public string? Connection { get; set; }

        public string? Connection1C { get; set; }
        public string? SupplierName { get; set; }

        public string? Type { get; set; }

        public string? Source { get; set; }

        public SourceSettings? SourceSettings { get; set; } 

        public SupplierConfig? SupplierConfigs { get; set; }

        public bool Equals(ProFileSupplier proFileSuppliers)
        {

            if (this.Connection != proFileSuppliers.Connection ||
                this.SupplierName != proFileSuppliers.SupplierName ||
                this.Type != proFileSuppliers.Type
                )
            {
                return false;
            }
            if ((this.SupplierConfigs == null ? 0 : 1) == (proFileSuppliers.SupplierConfigs == null ? 0 : 1))
            {
                if (this.SupplierConfigs != null && proFileSuppliers.SupplierConfigs != null)
                {

                    if (this.SupplierConfigs.Input != proFileSuppliers.SupplierConfigs.Input ||
                        this.SupplierConfigs.SupplierId != proFileSuppliers.SupplierConfigs.SupplierId ||
                        this.SupplierConfigs.Title != proFileSuppliers.SupplierConfigs.Title ||
                        this.SupplierConfigs.TitleLong != proFileSuppliers.SupplierConfigs.TitleLong ||
                        this.SupplierConfigs.Description != proFileSuppliers.SupplierConfigs.Description ||
                        this.SupplierConfigs.Vendor != proFileSuppliers.SupplierConfigs.Vendor ||
                        this.SupplierConfigs.VendorId != proFileSuppliers.SupplierConfigs.VendorId ||
                        this.SupplierConfigs.CategoriesProduct != proFileSuppliers.SupplierConfigs.CategoriesProduct ||
                        this.SupplierConfigs.CategoriesStart != proFileSuppliers.SupplierConfigs.CategoriesStart ||
                        this.SupplierConfigs.DocumentsStart != proFileSuppliers.SupplierConfigs.DocumentsStart ||
                        this.SupplierConfigs.Images != proFileSuppliers.SupplierConfigs.Images ||
                        this.SupplierConfigs.Image360 != proFileSuppliers.SupplierConfigs.Image360 ||
                        this.SupplierConfigs.AttributesStart != proFileSuppliers.SupplierConfigs.AttributesStart ||
                        this.SupplierConfigs.PackagesStart != proFileSuppliers.SupplierConfigs.PackagesStart ||
                        this.SupplierConfigs.Features != proFileSuppliers.SupplierConfigs.Features
                        )
                    {
                        return false;
                    }


                    if (this.SupplierConfigs.Documents != null && proFileSuppliers.SupplierConfigs.Documents != null)
                    {
                        if (this.SupplierConfigs.Documents.Type != proFileSuppliers.SupplierConfigs.Documents.Type ||
                            this.SupplierConfigs.Documents.Url != proFileSuppliers.SupplierConfigs.Documents.Url ||
                            this.SupplierConfigs.Documents.CertId != proFileSuppliers.SupplierConfigs.Documents.CertId ||
                            this.SupplierConfigs.Documents.CertNumber != proFileSuppliers.SupplierConfigs.Documents.CertNumber ||
                            this.SupplierConfigs.Documents.CertDescr != proFileSuppliers.SupplierConfigs.Documents.CertDescr ||
                            this.SupplierConfigs.Documents.File != proFileSuppliers.SupplierConfigs.Documents.File ||
                            this.SupplierConfigs.Documents.CertOrganizNumber != proFileSuppliers.SupplierConfigs.Documents.CertOrganizNumber ||
                            this.SupplierConfigs.Documents.CertOrganizDescr != proFileSuppliers.SupplierConfigs.Documents.CertOrganizDescr ||
                            this.SupplierConfigs.Documents.BlankNumber != proFileSuppliers.SupplierConfigs.Documents.BlankNumber ||
                            this.SupplierConfigs.Documents.StartDate != proFileSuppliers.SupplierConfigs.Documents.StartDate ||
                            this.SupplierConfigs.Documents.EndDate != proFileSuppliers.SupplierConfigs.Documents.EndDate ||
                            this.SupplierConfigs.Documents.Keywords != proFileSuppliers.SupplierConfigs.Documents.Keywords
                            )
                        {
                            return false;
                        }
                    }

                    if (this.SupplierConfigs.AttributesParam != null && proFileSuppliers.SupplierConfigs.AttributesParam != null)
                    {

                        if (this.SupplierConfigs.AttributesParam.NameAttribute != proFileSuppliers.SupplierConfigs.AttributesParam.NameAttribute ||
                            this.SupplierConfigs.AttributesParam.Unit != proFileSuppliers.SupplierConfigs.AttributesParam.Unit ||
                            this.SupplierConfigs.AttributesParam.Type != proFileSuppliers.SupplierConfigs.AttributesParam.Type ||
                            this.SupplierConfigs.AttributesParam.Value != proFileSuppliers.SupplierConfigs.AttributesParam.Value
                            )
                        {
                            return false;
                        }
                    }
                    if (this.SupplierConfigs.Packages != null && proFileSuppliers.SupplierConfigs.Packages != null)
                    {
                        if (this.SupplierConfigs.Packages.Barcode != proFileSuppliers.SupplierConfigs.Packages.Barcode ||
                            this.SupplierConfigs.Packages.Weight != proFileSuppliers.SupplierConfigs.Packages.Weight ||
                            this.SupplierConfigs.Packages.Volume != proFileSuppliers.SupplierConfigs.Packages.Volume ||
                            this.SupplierConfigs.Packages.PackQty != proFileSuppliers.SupplierConfigs.Packages.PackQty
                            )
                        {
                            return false;
                        }
                    }
                    if ((this.SupplierConfigs.productAttributeKeys == null ? 0 : this.SupplierConfigs.productAttributeKeys.Count) == (proFileSuppliers.SupplierConfigs.productAttributeKeys == null ? 0 : proFileSuppliers.SupplierConfigs.productAttributeKeys.Count))
                    {
                        for (int i = 0; i < (this.SupplierConfigs.productAttributeKeys == null ? 0 : this.SupplierConfigs.productAttributeKeys.Count); i++)
                        {
                            if (this.SupplierConfigs.productAttributeKeys[i].AttributeIdBD != proFileSuppliers.SupplierConfigs.productAttributeKeys[i].AttributeIdBD ||
                                this.SupplierConfigs.productAttributeKeys[i].KeySupplier != proFileSuppliers.SupplierConfigs.productAttributeKeys[i].KeySupplier ||
                                this.SupplierConfigs.productAttributeKeys[i].AttributeBDName != proFileSuppliers.SupplierConfigs.productAttributeKeys[i].AttributeBDName
                                )
                            {
                                return false;
                            }
                        }
                    }
                    if ((this.SupplierConfigs.Categories == null ? 0 : this.SupplierConfigs.Categories.Count) == (proFileSuppliers.SupplierConfigs.Categories == null ? 0 : proFileSuppliers.SupplierConfigs.Categories.Count))
                    {
                        for (int i = 0; i < (this.SupplierConfigs.Categories == null ? 0 : this.SupplierConfigs.Categories.Count); i++)
                        {
                            if (this.SupplierConfigs.Categories[i].VenderId != proFileSuppliers.SupplierConfigs.Categories[i].VenderId ||
                                this.SupplierConfigs.Categories[i].Title != proFileSuppliers.SupplierConfigs.Categories[i].Title ||
                                this.SupplierConfigs.Categories[i].Description != proFileSuppliers.SupplierConfigs.Categories[i].Description ||
                                this.SupplierConfigs.Categories[i].SubCategories != proFileSuppliers.SupplierConfigs.Categories[i].SubCategories
                                )
                            {
                                return false;
                            }
                        }
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
