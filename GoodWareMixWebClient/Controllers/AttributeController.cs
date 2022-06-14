using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using GoodWareMixWebClient.Services;
using GoodWareMixWebClient.ViewModel;
using GoodWareMixWebClient.Views.Attribute;

namespace GoodWareMixWebClient.Controllers
{
    public class AttributeController : Controller
    {
        private readonly ILogger<AttributeController> _logger;
        private readonly INotyfService _notyf;
        public AttributeController(ILogger<AttributeController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        // GET: AttributeController
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
            string url = $"{Connection.DefaultConnection}api/Attribute?PageSize={PageSize}";

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
                AttributeViewModel attributeViewModel = new AttributeViewModel { AttributePagedResponse = JsonConvert.DeserializeObject<PagedResponse<List<AttributeEntity>>>(json) };
                return View(attributeViewModel);
            
        }

        #region Редактирование аттрибута
        public async Task<IActionResult> EditAttributeModal(string attributeID ,string[] autocomplete)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Attribute/{attributeID}");

            string json = await response.Content.ReadAsStringAsync();
            var attr = JsonConvert.DeserializeObject<AttributeEntity>(json);

            return PartialView(attr);
        }

        public async Task<IActionResult> SaveEditChanges(string[] autocomplete, string NameAttribute, bool IsVerified)
        {
            AttributeHelper helper = new AttributeHelper();
            HttpClient client = new HttpClient();

            helper.attributeUpdate = NameAttribute;
            helper.attributeList = autocomplete.ToList();

            var content = new StringContent(JsonConvert.SerializeObject(helper), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{Connection.DefaultConnection}api/Attribute", content);

            if (response.IsSuccessStatusCode) { _notyf.Success($"Аттрибут успешно отредактирован", 10); }
            else { _notyf.Error("Ошибка редактирования", 10); }

            return RedirectToAction("Index", "Attribute");
        }
        #endregion

        #region Удаление аттрибута
        public async Task<IActionResult> DeleteAttributeModal(string nameAttribute, string id)
        {
            AttributeEntity attribute = new AttributeEntity()
            {
                NameAttribute = nameAttribute,
                Id = id
            };
            return PartialView(attribute);
        }
        public async Task<IActionResult> DeleteConfirm(string nameAttribute, string id)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.DeleteAsync($"{Connection.DefaultConnection}api/Attribute/{id}");

            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Аттрибут {nameAttribute} успешно удален", 10);
            }
            else
            {
                _notyf.Error($"Ошибка удаления. {response.StatusCode}", 10);
            }
            return RedirectToAction("Index", "Attribute");
        }
        #endregion
    }
}
