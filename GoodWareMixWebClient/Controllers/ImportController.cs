using AspNetCoreHero.ToastNotification.Abstractions;
using GoodWareMixWebClient.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace WebClientMakup.Controllers
{
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _logger;
        private readonly INotyfService _notyf;

        public ImportController(ILogger<ImportController> logger, INotyfService notyf)
        {
            _logger = logger;
            _notyf = notyf;
        }

        private IFormFile formFile;

        // View Import
        [RequestSizeLimit(Int32.MaxValue)]

        public async Task<IActionResult> Index(string supplierName, IFormFile file , string[] SelectAttribute , string[] AttributeKey)
        {
            if (file != null)
            {
                FileInfo fileInfo = new FileInfo(file.FileName);
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);
                client.BaseAddress = new Uri(Connection.DefaultConnection);
                using var requestContext = new MultipartFormDataContent();
                //file.OpenReadStream();
                //var fileStream = System.IO.File.OpenRead((Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), file.FileName)));
                requestContext.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                HttpResponseMessage message =  await client.PostAsync($"{Connection.DefaultConnection}api/product/{supplierName}", requestContext);

                string RepsonseAsJson = await message.Content.ReadAsStringAsync();
                if (message.IsSuccessStatusCode)
                {
                    _notyf.Success("Успешный импорт", 10);
                }
                else
                {
                    _notyf.Error("Ошибка импорта", 10);
                }
                //List<string> ls = JsonConvert.DeserializeObject<List<string>>(RepsonseAsJson);
                //foreach (var item in ls)
                //{
                //    ViewBag.Result += item+"\n";
                //}


                HttpClient client1 = new HttpClient();
                client1.BaseAddress = new Uri(Connection.DefaultConnection);
                HttpResponseMessage response = await client1.GetAsync($"{Connection.DefaultConnection}api/Supplier/{supplierName}");
                string json = await response.Content.ReadAsStringAsync();
                var supplier = JsonConvert.DeserializeObject<ProFileSupplier>(json);
                return View(supplier);
                //return View();
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
        
        //public async void StartAction(string supplierName)
        //{
        //    #region WebSocket
        //    //using (ClientWebSocket socketClient = new ClientWebSocket())
        //    //{
        //    //    Uri serviceUri = new Uri($"{Connection.DefaultConnection}api/send");
        //    //    var cts = new CancellationTokenSource();
        //    //    cts.CancelAfter(TimeSpan.FromSeconds(120));
        //    //    try
        //    //    {
        //    //        await socketClient.ConnectAsync(serviceUri, cts.Token);
        //    //        var n = 0;
        //    //        while (socketClient.State == WebSocketState.Open)
        //    //        {
        //    //            _notyf.Information("opened", 10);

        //    //            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{Connection.DefaultConnection}api/Product?name={supplierName}"));
        //    //            await socketClient.SendAsync(byteToSend, WebSocketMessageType.Text, true, cts.Token);
        //    //            var responseBuffer = new byte[1024];
        //    //            var offset = 0;
        //    //            var packet = 1024;
        //    //            while (true)
        //    //            {
        //    //                ArraySegment<byte> byteRecieved = new ArraySegment<byte>(responseBuffer, offset,packet);
        //    //                WebSocketReceiveResult responseResult = await socketClient.ReceiveAsync(byteRecieved, cts.Token);
        //    //                var responseMessage= Encoding.UTF8.GetString(responseBuffer, offset, responseResult.Count);
        //    //            }
        //    //        }
        //    //    }
        //    //    catch (WebSocketException ex)
        //    //    {
        //    //        _notyf.Error($"Ошибка запуска {ex.Message}", 10);
        //    //    }
        //    //}
        //    #endregion

        //    //_notyf.Success($"Задача запущена", 10);
        //    HttpClient client = new HttpClient();
        //    client.Timeout = TimeSpan.FromMinutes(5);
        //    HttpContent? content = null;
        //    HttpResponseMessage response = await client.PostAsync($"{Connection.DefaultConnection}api/Product?name={supplierName}", content);
        //    //if (response.IsSuccessStatusCode)
        //    //{
        //    //    _notyf.Success($"Задача выполнена", 10);
        //    //}
        //    //else
        //    //{
        //    //    _notyf.Error("Ошибка запуска", 10);
        //    //}
        //}

        
        public async Task<List<string>> Calculate3()
        {
            //BsonArray bsonArray = new BsonArray();
            //BsonDocument doc = new BsonDocument { { "Value", "1" } };
            //BsonDocument doc1 = new BsonDocument { { "Value", "2" } };
            //bsonArray.Add(doc);
            //bsonArray.Add(doc1);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://172.16.41.203:45455/");
            HttpResponseMessage response = await client.GetAsync($"http://172.16.41.203:45455/api/Attribute");
            string json = await response.Content.ReadAsStringAsync();
            var attribute = JsonConvert.DeserializeObject<PagedResponse<List<AttributeEntity>>>(json);
            List<string> list = new List<string>();
            foreach (var item in attribute.Data)
            {
                list.Add(item.NameAttribute);
            }
            return list;
        }


        // Upload Import
        
        public async void Upload(string supplier, IFormFile file)
        {
            var s = file.OpenReadStream().ToString();
            FileInfo fileInfo = new FileInfo(file.FileName);
            using (FileStream stream = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), file.FileName), FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ViewBag.Res = await reader.ReadToEndAsync();
                }
            }
        }

        // Upload To Base Import
        public async Task<IActionResult> UploadToBase(string supplierName, IFormFile file)
        {
            file = formFile;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Connection.DefaultConnection);
            
            using var requestContext = new MultipartFormDataContent();
            var fileStream = System.IO.File.OpenRead(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), file.FileName));
            requestContext.Add(new StreamContent(fileStream), "file", file.FileName);
            await client.PostAsync($"{Connection.DefaultConnection}api/product/{supplierName}", requestContext);
            return RedirectToAction("Index", "Home");

        }
    }
}
