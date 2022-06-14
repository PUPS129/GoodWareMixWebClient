using GoodWareMixWebClient.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

using GoodWareMixWebClient.Services;
using GoodWareMixWebClient.Models;

namespace WebClientMakup.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }


        // View Product




        public async Task<IActionResult> Index(int PageNumber, string Search, int PageSize, string[] Attribute, string InternalCodeCheck, string autocompleteSupplier, bool PaginationBool, string DisplayMode, string OrderBy)
        {
            ViewBag.CountFilter = 0;
            if (DisplayMode != null)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "DisplayMode", DisplayMode); //закинуть в куки
                ViewBag.DisplayMode = DisplayMode; // SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "DisplayMode"); // получить куки
            }
            else 
            {
                if (SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "DisplayMode") == null)
                {
                    ViewBag.DisplayMode = "1";
                }
                else
                {
                    ViewBag.DisplayMode = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "DisplayMode");
                }
                
            }

            if (!PaginationBool)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Search", Search); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "PageSize", PageSize); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Attribute", Attribute); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "InternalCodeCheck", InternalCodeCheck); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Supplier", autocompleteSupplier); //закинуть в куки
                SessionHelper.SetObjectAsJson(HttpContext.Session, "OrderBy", OrderBy); //закинуть в куки
            }
            else
            {
                Search = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Search"); // получить куки
                PageSize = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "PageSize"); // получить куки
                Attribute = SessionHelper.GetObjectFromJson<string[]>(HttpContext.Session, "Attribute"); // получить куки
                InternalCodeCheck = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "InternalCodeCheck"); // получить куки
                autocompleteSupplier = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "Supplier"); // получить куки
                OrderBy = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "OrderBy"); // получить куки
            }
            HttpClient client = new HttpClient();


            string InternalCodeOnly = "";
            string PageNumberSum = "";
            string url = $"{Connection.DefaultConnection}api/Product?PageSize={PageSize}";
            string att = "";
            string att1 = "";
            if (InternalCodeCheck == "on")
            {
                ViewBag.CountFilter += 1;
                ViewBag.CheckBoxStatus = true;
                url += "&InternalCodeCheckBool=true";
            }
            if (PaginationBool)
            {
                url += "&PageNumber="+PageNumber;
            }
            if (!string.IsNullOrEmpty(Search))
            {
                ViewBag.Search = Search;
                url += $"&Search={Search}";
            }
            if (!string.IsNullOrEmpty(autocompleteSupplier))
            {
                ViewBag.CountFilter += 1;
                ViewBag.Supplier = autocompleteSupplier;
                url += $"&Supplier={autocompleteSupplier}";
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                url += $"&OrderBy={OrderBy}";
            }
            if (Attribute.Count() != 0)
            {
                ViewBag.CountFilter += Attribute.Count();
                BsonArray bsonArray = new BsonArray();
                List<string> test1 = new List<string>();
                for (int i = 0; i < Attribute.Length; i++)
                {
                    string buf = Attribute[i].Split(';')[0];
                    string buf1 = Attribute[i].Split(';')[1];
                    ViewBag.SelectedItem = buf1;
                    test1.Add(buf);
                }
                for (int i = 0; i < test1.Count; i++)
                {
                    if (i == 0)
                    {
                        att = att + "attributeName=" + test1[i];
                    }
                    else
                    {
                        att = att + "&attributeName=" + test1[i];
                    }
                    att1 = att1 + "&Attributes=" + Attribute[i];

                }
                List<string[]> list123 = await ListValueAttribut(att);
                for (int i = 0; i < Attribute.Length; i++)
                {
                    string buf = Attribute[i].Split(';')[0];
                    BsonArray bsonArray1 = new BsonArray();


                    foreach (var item in list123[i])
                    {
                        if (item != null)
                        {


                            BsonDocument? Bufbson = new BsonDocument {
                                    { "Value", item }
                                        };

                            bsonArray1.Add(Bufbson);
                        }
                    }
                    BsonDocument nested = new BsonDocument {
                                    { "Key", buf } ,
                                    { "AttributeValue" ,  bsonArray1} };
                    
                    
                    bsonArray.Add(nested);

                }
                ViewData["AttributeKey"] = bsonArray;
                url += att1;
            }

            HttpResponseMessage response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PagedResponse<List<Product>>>(json);
            return View(list);


        }

        // View Product About
        public async Task<IActionResult> About(string productID, string url)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Connection.DefaultConnection);
            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/product/{productID}");
            string json = await response.Content.ReadAsStringAsync();
            Product product = JsonConvert.DeserializeObject<Product>(json);
            ViewBag.Url = url;

            return View(product);
        }

        public async Task<IActionResult> ChangeDisplay (int Display)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "DisplayMode", Display); //закинуть в куки
            ViewBag.DisplayMode = SessionHelper.GetObjectFromJson<string>(HttpContext.Session, "DisplayMode"); // получить куки
            return RedirectToAction("Index");
        }
        
        public async Task<List<string>> ValueAttribut(string number1)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Connection.DefaultConnection}");
            HttpResponseMessage? response = await client.GetAsync($"{Connection.DefaultConnection}api/AttributeValues/{number1}");
            string json = await response.Content.ReadAsStringAsync();
            List<string> list = new List<string>();
            if (json != null)
            {
                list = JsonConvert.DeserializeObject<List<string>>(json);
            }

            return list;
        }
        
        public async Task<List<string[]>> ListValueAttribut(string number1)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Connection.DefaultConnection}");
            HttpResponseMessage? response = await client.GetAsync($"{Connection.DefaultConnection}api/AttributeValues?{number1}");
            string json = await response.Content.ReadAsStringAsync();
            List<string[]> list = new List<string[]>();
            if (json != null)
            {
                list = JsonConvert.DeserializeObject<List<string[]>>(json);
            }

            return list;
        }
        
        public void SessionHelperIndex(string Search, int PageSize, string[] Attribute)
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Attribute", Attribute);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Search", Search);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "Attribute", Attribute);
            //this.Index(0, Search, PageSize, Attribute);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
