using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Diagnostics;
using GoodWareMixWebClient.Model;
using GoodWareMixWebClient.Services;
using GoodWareMixWebClient.Models;

namespace GoodWareMixWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger; //
        }

        public async Task<IActionResult> Index(Uri PageNumber, string Search, int PageSize)
        {
            HttpClient client = new HttpClient();

            if (PageNumber == null)
            {
                if (Search != null)
                {
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "Search", Search);
                    HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier?PageSize={100}&Search={Search}");
                    string json = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
                    return View(list);
                }
                else
                {

                    HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier?PageSize={100}");
                    string json = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
                    return View(list);
                }
            }
            else
            {
                HttpResponseMessage response = await client.GetAsync(PageNumber);
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
                return View(list);

            }
        }

        public async Task<IActionResult> Privacy(Uri PageNumber, string Search, int PageSize, string[] Attribute, string InternalCodeCheck, string autocompleteSupplier)
        {
            HttpClient client = new HttpClient();

            if (PageNumber == null)
            {
                string InternalCodeOnly = "";
                string url = $"{Connection.DefaultConnection}api/Product?PageSize={PageSize}{PageNumber}";
                string att = "";
                string att1 = "";
                if (InternalCodeCheck == "on")
                {
                    url += "&InternalCodeCheckBool=true";
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    url += $"&Search={Search}";
                }
                

                HttpResponseMessage response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<Product>>>(json);
                return View(list);
            }
            else
            {
                HttpResponseMessage response = await client.GetAsync(PageNumber);
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<Product>>>(json);
                return View(list);
            }


        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}