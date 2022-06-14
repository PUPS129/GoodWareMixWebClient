using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Model;
using GoodWareMixWebClient.Model.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using GoodWareMixWebClient.Services;
using GoodWareMixWebClient.ViewModel;

namespace GoodWareMixWebClient.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly INotyfService _notyf;
        public TaskController(ILogger<TaskController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        // GET: TaskController
        public async Task<IActionResult> AddEditTaskModal(string nameTask)
        {
            if (!string.IsNullOrEmpty(nameTask))
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);

                //HttpResponseMessage responseTask = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask?PageSize={25}");
                var responseTask1 =  client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{nameTask}").Result.Content.ReadAsStringAsync().Result;
                //var js = await responseTask.Content.ReadAsStringAsync();
                //var js2 = await responseTask1.Content.ReadAsStringAsync();
                var task = JsonConvert.DeserializeObject<PagedResponse<List<SchedulerTask>>>(responseTask1);
                //var toSend = task.Data.FirstOrDefault(x => x.NameTask == nameTask);
                return PartialView(task);
            }
            else
            {
                SchedulerTask task = new SchedulerTask();
                return PartialView(task);
            }

        }

        public async Task <IActionResult> AddEditTask(SchedulerTask task, string supplierId)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            //HttpResponseMessage getTask = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{task.NameTask}");
            //var jsonTask = await getTask.Content.ReadAsStringAsync();
            //var RecievedTask = JsonConvert.DeserializeObject<SchedulerTask>(jsonTask);

            HttpResponseMessage getSupplier = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{task.SupplierId}");
            var jsonSupp = await getSupplier.Content.ReadAsStringAsync();
            var RecievedSupp = JsonConvert.DeserializeObject<ProFileSupplier>(jsonSupp);

            string url = $"{Connection.DefaultConnection}api/SchedulerTask/{task.NameTask}";

            SchedulerTask schedulerTask = new SchedulerTask()
            {
                SupplierId = RecievedSupp.Id,
                NameTask = task.NameTask,
                Description = task.Description,
                CronExpression = task.CronExpression,
                StartAt = task.StartAt,
                NextDateTask = task.NextDateTask,
                IsEnable = false,
            };

            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(schedulerTask, settings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var FindTask = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{task.NameTask}");
            HttpResponseMessage response = new HttpResponseMessage(); // await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{task.Supplier.SupplierName}");
            if (FindTask.StatusCode != System.Net.HttpStatusCode.NoContent && FindTask.IsSuccessStatusCode)
            {   // SchedulerTask
                string putUrl = $"{Connection.DefaultConnection}api/SchedulerTask/{schedulerTask.NameTask}";
                var result = await client.PutAsync(putUrl, content);
                if (result.IsSuccessStatusCode)
                {
                    _notyf.Success($"Задача {schedulerTask.NameTask} успешно обновлена", 10);
                }
                else
                {
                    _notyf.Error($"Ошибка обновления. {result.StatusCode}", 10);
                }
            }
            else
            {
                string postUrl = $"{Connection.DefaultConnection}api/SchedulerTask";
                response = await client.PostAsync(postUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    _notyf.Success($"Задача {schedulerTask.NameTask} успешно добавлена", 10);
                }
                else
                {
                    _notyf.Error($"Ошибка добавления. {response.StatusCode}", 10);
                }
            }
            return RedirectToAction("Index","Task");
        }

        //public async Task<IActionResult> EditTask(string supplierName)
        //{

        //    HttpClient client = new HttpClient();
        //    client.Timeout = TimeSpan.FromMinutes(5);
        //    HttpContent? content = null;


        //    HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");


        //    var js = await response.Content.ReadAsStringAsync();
        //    ProFileSupplier supplier = JsonConvert.DeserializeObject<ProFileSupplier>(js);
        //    SchedulerTask task = new SchedulerTask()
        //    {
        //        StartAt = DateTime.Now,
        //        IsEnable = false,
        //        Supplier = supplier,
        //    };



        //    return View(task);
        //}

        public async Task<IActionResult> Index(int PageNumber, string Search, int PageSize, bool PaginationBool, string supplierName)
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
            string url = $"{Connection.DefaultConnection}api/SchedulerTask?PageSize={PageSize}";

            if (PaginationBool)
            {
                url += "&PageNumber=" + PageNumber;
            }
            if (!string.IsNullOrEmpty(Search))
            {
                ViewBag.Search = Search;
                url += $"&Search={Search}";
            }
            ProFileSupplier foundSupplier = new ProFileSupplier();
            if (supplierName != null)
            {
                HttpResponseMessage supplierResponse = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");
                string json = await supplierResponse.Content.ReadAsStringAsync();
                foundSupplier = JsonConvert.DeserializeObject<ProFileSupplier>(json);
            }
            
            HttpResponseMessage response = await client.GetAsync(url);
            var js = await response.Content.ReadAsStringAsync();

            var recievedTask = JsonConvert.DeserializeObject<PagedResponse<List<SchedulerTask>>>(js);
            if (foundSupplier.Id != null)
            {
                recievedTask.Data = recievedTask.Data.Where(x => x.SupplierId == foundSupplier.SupplierName).ToList();
            }
            
            SchedulerTaskViewModel schedulerTaskViewModel = new SchedulerTaskViewModel
            {
                SchedulerTaskPagedResponse = recievedTask,
                ProFileSupplier = foundSupplier,
            };
            return View(schedulerTaskViewModel);
        }


        public async Task<IActionResult> StartTask(string taskName)
        {

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);


            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{taskName}");


            var js = await response.Content.ReadAsStringAsync();
            SchedulerTask task = JsonConvert.DeserializeObject<SchedulerTask>(js);

            string url = $"{Connection.DefaultConnection}api/Product/startScheduler/{taskName}";
            var content = new StringContent(js, Encoding.UTF8, "application/json");
            var r = await client.GetAsync(url);

            if (r.IsSuccessStatusCode)
            {
                _notyf.Success($"Задача {taskName} запущена", 10);
                return RedirectToAction("Index", "Task");
            }
            else
            {
                _notyf.Error("Ошибка запуска", 10);
                return RedirectToAction("Index", "Task");
            }

        }

        public async Task<IActionResult> StopTask(string taskName)
        {

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);


            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{taskName}");


            var js = await response.Content.ReadAsStringAsync();
            SchedulerTask task = JsonConvert.DeserializeObject<SchedulerTask>(js);

            string url = $"{Connection.DefaultConnection}api/Product/stopScheduler/{taskName}";
            var content = new StringContent(js, Encoding.UTF8, "application/json");
            var r = await client.GetAsync(url);
            if (r.IsSuccessStatusCode)
            {
                _notyf.Success($"Задача {taskName} остановлена", 10);
                return RedirectToAction("Index", "Task");
            }
            else
            {
                _notyf.Error("Ошибка остановки", 10);
                return RedirectToAction("Index", "Task");
            }


        }

        public async Task<IActionResult> DeleteTaskModal(string taskName)
        {
            SchedulerTask schedulerTask = new SchedulerTask()
            {
                NameTask = taskName
            };
            return PartialView(schedulerTask);

            
        }

        public async Task<IActionResult> DeleteTaskConfirm(string nameTask)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            HttpResponseMessage response = await client.DeleteAsync($"{Connection.DefaultConnection}api/SchedulerTask/{nameTask}");
            if (response.IsSuccessStatusCode)
            {
                _notyf.Success($"Задача {nameTask} успешно удалена", 10);
                return RedirectToAction("Index", "Task");
            }
            else
            {
                _notyf.Error("Ошибка удаления", 10);
                return RedirectToAction("Index", "Task");
            }
        }

        // Проверяет имя задчи при добавлении
        public async Task<bool> GetTask(string NameTask)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/SchedulerTask/{NameTask}");
            if (response.StatusCode != System.Net.HttpStatusCode.NoContent && response.IsSuccessStatusCode && NameTask != null)
            {
                _notyf.Error($"Задача с таким именем уже существует.", 10);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<PagedResponse<List<ProFileSupplier>>> GetSupplier(string Search)
       {
            if (Search != null)
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Supplier?PageSize=25&Search={Search}");
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
                return list;
            }
            else
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{Connection.DefaultConnection}api/Attribute?PageSize=25");
                string json = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<PagedResponse<List<ProFileSupplier>>>(json);
                return list;
            }
        }
    }
}
