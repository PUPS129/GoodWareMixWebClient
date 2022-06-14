using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Model;
using GoodWareMixWebClient.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GoodWareMixWebClient.Services;

namespace WebClientMakup.Controllers
{
    public class LoggingController : Controller
    {
        private readonly ILogger<LoggingController> _logger;
        private readonly INotyfService _notyf;
        public LoggingController(ILogger<LoggingController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        // GET: LoggingController
        public async Task <IActionResult> Index(int PageNumber, string Search, int PageSize, bool PaginationBool)
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
            string url = $"{Connection.DefaultConnection}api/Log?PageSize={PageSize}";

            if (PaginationBool)
            {
                url += "&PageNumber=" + PageNumber;
            }
            if (!string.IsNullOrEmpty(Search))
            {
                url += $"&Search={Search}";
            }

            HttpResponseMessage response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<PagedResponse<List<Log>>>(json);
            return View(list);            

        }

        public async Task<IActionResult> DeleteLogModal()
        {
            return PartialView();
        }


        public async Task<IActionResult> Delete()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Connection.DefaultConnection}");

            HttpResponseMessage response = await client.DeleteAsync($"{Connection.DefaultConnection}api/Log/DeleteLogs");
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success("Журнал успешно очищен", 10);
            }
            else
            {
                _notyf.Error("Ошибка очистки", 10);
            }
            return RedirectToAction("Index", "Logging");
        }
    }
}
