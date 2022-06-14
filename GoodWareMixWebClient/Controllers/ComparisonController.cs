using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace GoodWareMixWebClient.Controllers
{
    public class ComparisonController : Controller
    {
        private readonly ILogger<ComparisonController> _logger;
        private readonly INotyfService _notyf;
        public ComparisonController(ILogger<ComparisonController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        private IFormFile formFile;

        [RequestSizeLimit(Int32.MaxValue)]
        public async Task<IActionResult> Index(string supplierName, IFormFile file)
        {
            if (file != null)
            {
                FileInfo fileInfo = new FileInfo(file.FileName);
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);
                client.BaseAddress = new Uri(Connection.DefaultConnection);
                using var requestContext = new MultipartFormDataContent();

                requestContext.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                HttpResponseMessage message = await client.PostAsync($"{Connection.DefaultConnection}api/product/InternalCodeBinding/{supplierName}", requestContext);
                string RepsonseAsJson = await message.Content.ReadAsStringAsync();
                if (message.IsSuccessStatusCode)
                {
                    _notyf.Success("Импорт запущен", 10);
                    //List<string> ls = JsonConvert.DeserializeObject<List<string>>(RepsonseAsJson);
                    //foreach (var item in ls)
                    //{
                    //    ViewBag.Result += item + "\n";
                    //}
                }
                else
                {
                    _notyf.Error($"Ошибка запуска", 10);
                }
                return RedirectToAction("Index", "Supplier");
            }
            else
            {
                if (supplierName != null)
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Connection.DefaultConnection);
                    HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");
                    string json = await response.Content.ReadAsStringAsync();
                    var supplier = JsonConvert.DeserializeObject<ProFileSupplier>(json);
                    if (supplier != null)
                    {
                        return View(supplier);
                    }
                    else { return View(); }
                }
                else { return View(); }
            }
        }


        // GET: ComparsionController
        [HttpPost]
        public async Task<IActionResult> LoadFile(string supplierName, IFormFile file)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            HttpContent? content = null;
            HttpContent cn = (HttpContent)file;


            HttpResponseMessage response = await client.PostAsync($"{Connection.DefaultConnection}api/InternalCodeBinding?supplierName={supplierName}", content);

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Задача для {supplierName} запущена", 10);
                return RedirectToAction("Index", "Supplier");
            }
            else
            {
                _notyf.Error("Ошибка запуска", 10);
                return RedirectToAction("Index", "Supplier");
            }
        }

        // GET: ComparsionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ComparsionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ComparsionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ComparsionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ComparsionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ComparsionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ComparsionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
