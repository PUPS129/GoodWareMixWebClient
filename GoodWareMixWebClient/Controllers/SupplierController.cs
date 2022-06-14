using GoodWareMixWebClient.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Services;
using GoodWareMixWebClient.Model.Settings;
using GoodWareMixWebClient.Model.Entity;
using GoodWareMixWebClient.Models;

namespace WebClientMakup.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly INotyfService _notyf;
        public SupplierController(ILogger<SupplierController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }


        
        public async Task<IActionResult> StartAction(string supplierId, string supplierName)
        {

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpContent? content = null;
            HttpResponseMessage response = await client.PostAsync($"{Connection.DefaultConnection}api/Product?name={supplierId}", content);

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Задача для {supplierName} запущена", 10);
                return RedirectToAction("Index", "Supplier");
            }
            else
            {
                _notyf.Error($"Ошибка запуска. {response.StatusCode}", 10);
                return RedirectToAction("Index", "Supplier");
            }
        }

        
        public async Task<IActionResult> Index(int PageNumber, string Search, int PageSize, bool PaginationBool)
        {
            if (!PaginationBool)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Search", Search); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "PageSize", PageSize); //закинуть в куки
            }
            else
            {
                Search = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Search"); // получить куки
                PageSize = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "PageSize"); // получить куки
            }
            HttpClient client = new HttpClient();
            string PageNumberSum = "";
            string url = $"{Connection.DefaultConnection}api/Supplier?PageSize={PageSize}";

            if (PaginationBool)
            {
                url += "&PageNumber=" + PageNumber;
            }
            if (!string.IsNullOrEmpty(Search))
            {
                ViewBag.Search = Search;
                url += $"&Search={Search}";
            }

            HttpResponseMessage response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
            return View(list);
        }


        #region Добавление / Редактирование поставщика
        public async Task<IActionResult> UpdateSupplierInBase(SourceSettings sourceSettings, ProFileSupplier supp, SupplierConfig config, string[] autocomplete, string[] AttributeKey)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Connection.DefaultConnection);

            List<ProductAttributeKey> Attributes = new List<ProductAttributeKey>();
            for (int i = 0; i < autocomplete.Length; i++)
            {
                Attributes.Add(new ProductAttributeKey()
                {
                    KeySupplier = AttributeKey[i],
                    AttributeBDName = autocomplete[i]
                });
            }


            ProFileSupplier updatedSupplier = new ProFileSupplier
            {
                Type = supp.Type,
                Connection = supp.Connection,
                Source = supp.Source,
                SupplierName = supp.SupplierName,
                Id = "string",
                SourceSettings = new SourceSettings()
                {
                    Url = sourceSettings.Url,
                    Prefix = sourceSettings.Prefix,
                    MethodType = sourceSettings.MethodType,
                    Header = sourceSettings.Header,
                    Body = sourceSettings.Body,
                    CountPage = sourceSettings.CountPage,
                    FileEncoding = sourceSettings.FileEncoding,
                    StartPage = sourceSettings.StartPage,
                },
                SupplierConfigs = new SupplierConfig()
                {
                    Input = config.Input,
                    SupplierId = null,
                    Title = config.Title,
                    TitleLong = config.TitleLong,
                    Description = config.Description,
                    Vendor = config.Vendor,
                    VendorId = config.VendorId,
                    Images = config.Images,
                    Image360 = config.Image360,
                    //Videos = config.Videos,
                    Features = config.Features,
                    CategoriesProduct = null,
                    CategoriesStart = config.CategoriesStart,
                    AttributesURL = config.AttributesURL,
                    AttributesStart = config.AttributesStart,
                    AttributesParam = new ProductAttributesBuf()
                    {
                        AttributesInputURL = config.AttributesParam.AttributesInputURL,
                        NameAttribute = config.AttributesParam.NameAttribute,
                        Unit = config.AttributesParam.Unit,
                        Type = config.AttributesParam.Type,
                        Value = config.AttributesParam.Value,
                    },

                    DocumentsStart = config.DocumentsStart,
                    DocumentsURL = config.DocumentsURL,
                    Documents = new DocumentConfig()
                    {
                        Type = config.Documents.Type,
                        Url = config.Documents.Url,
                        CertId = config.Documents.CertId,
                        CertNumber = config.Documents.CertNumber,
                        CertDescr = config.Documents.CertDescr,
                        File = config.Documents.File,
                        CertOrganizNumber = config.Documents.CertOrganizNumber,
                        CertOrganizDescr = config.Documents.CertOrganizDescr,
                        BlankNumber = config.Documents.BlankNumber,
                        StartDate = config.Documents.StartDate,
                        EndDate = config.Documents.EndDate,
                        Keywords = config.Documents.Keywords,
                    },



                    PackagesStart = config.PackagesStart,
                    Packages = new SupplierConfigPackage()
                    {
                        Barcode = config.Packages.Barcode,
                        Type = config.Packages.Type,
                        Height = config.Packages.Height,
                        Width = config.Packages.Width,
                        Length = config.Packages.Length,
                        Depth = config.Packages.Depth,
                        Weight = config.Packages.Weight,
                        Volume = config.Packages.Volume,
                        PackQty = config.Packages.PackQty,
                    },

                    productAttributeKeys = Attributes,

                }
            };


            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(updatedSupplier, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var FindSupp = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supp.Id}");
            HttpResponseMessage response = new HttpResponseMessage();
            if (FindSupp.StatusCode == System.Net.HttpStatusCode.OK && supp.Id != null)
            {
                string putUrl = $"{Connection.DefaultConnection}api/Supplier/{updatedSupplier.SupplierName}";
                response = await client.PutAsync(putUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Поставщик {updatedSupplier.SupplierName} успешно обновлен", 10);
                }
                else
                {
                    _notyf.Error($"Ошибка обновления. {response.StatusCode}", 10);
                }
            }
            else
            {
                string postUrl = $"{Connection.DefaultConnection}api/Supplier";
                response = await client.PostAsync(postUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Поставщик {updatedSupplier.SupplierName} успешно добавлен", 10);
                }
                else
                {
                    _notyf.Error($"Ошибка добавления. {response.StatusCode}", 10);
                }
            }



            return RedirectToAction("Index", "Supplier");
        }

        public async Task<ActionResult> EditSupplier(string supplierName)
        {
            
            //    ProFileSupplier supplier = new ProFileSupplier
            //    {
            //        SupplierName = name,
            //    };


            HttpClient client = new HttpClient();
            ProFileSupplier proFileSupplier = new ProFileSupplier();
            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");

            string json = await response.Content.ReadAsStringAsync();
            var supplier = JsonConvert.DeserializeObject<ProFileSupplier>(json);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                proFileSupplier.SupplierName = supplierName;
                return View(proFileSupplier);
            }
            else
            {
                return View(supplier);
            }
        }
        #endregion

        #region Удаление поставщика
        public async Task<IActionResult> DeleteSupplierModal(string supplierName)
        {
            ProFileSupplier supplier = new ProFileSupplier()
            {
                SupplierName = supplierName,
            };
            return PartialView(supplier);
        }
        public async Task<IActionResult> DeleteConfirm(string supplierName)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"{supplierName} успешно удален", 10);
            }
            else
            {
                _notyf.Error($"Ошибка удаления. {response.StatusCode}", 10);
            }
            return RedirectToAction("Index", "Supplier");
        }
        #endregion

        #region Удаление всех продуктов поставщиков
        public async Task<IActionResult> DeleteSupplierProductsModal(string supplierId, string supplierName)
        {
            ProFileSupplier supplier = new ProFileSupplier()
            {
                Id = supplierId,
                SupplierName = supplierName,
            };
            return PartialView(supplier);
        }

        public async Task<IActionResult> DeleteSupplierProductConfirm(string supplierId, string supplierName)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync($"{Connection.DefaultConnection}api/Supplier?supplierId={supplierId}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Удаление товаров {supplierName} начато", 10);
            }
            else
            {
                _notyf.Error($"Ошибка удаления товаров. {response.StatusCode}", 10);
            }
            return RedirectToAction("Index", "Supplier");
        }
        #endregion

        public async Task<ActionResult> SearchSupplier(string Search)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{Search}");

            string json = await response.Content.ReadAsStringAsync();
            PagedResponse<List<ProFileSupplier>>? list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);

            return RedirectToAction("Index", "Supplier");
        }

        public async Task<PagedResponse<List<AttributeEntity>>> GetAttribute(string Search)
        {
            if (Search != null)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Attribute?PageSize=25&Search={Search}");
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<AttributeEntity>>>(json);
                return list;
            }
            else
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Attribute?PageSize=25");
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<AttributeEntity>>>(json);
                return list;
            }



        }

        public void SessionHelperIndex(string Search, int PageSize)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Search", Search);
        }

        // Проверяет имя поставщика при добавлении
        public async Task<bool> GetSupplier(string supplierName)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier?PageSize=100");
            string json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
            var supplier =  list.Data.FirstOrDefault(x => x.SupplierName == supplierName);
            if (supplier != null)
            {
                _notyf.Error($"Поставщик с таким именем уже существует.", 10);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> PrefixSearch(string code)
        {
            HttpClient client = new HttpClient();
            PrefixClass attribute = new PrefixClass();
            HttpResponseMessage response = await client.GetAsync($"{Connection.PrefixApiConnection}prefix?code={code}");
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Код найден", 10);
                string json = await response.Content.ReadAsStringAsync();
                attribute = JsonConvert.DeserializeObject<PrefixClass>(json);
                return attribute.Prefix;
            }
            else
            {
                _notyf.Error("Код не найден", 10);
                return string.Empty;
            }
            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
