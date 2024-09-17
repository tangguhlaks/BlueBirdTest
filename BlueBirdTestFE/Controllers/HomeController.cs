using BlueBirdTestFE.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace BlueBirdTestFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private string apiurl = "";

        public HomeController(ILogger<HomeController> logger,IConfiguration configuration,HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
            apiurl = _configuration["BackendUrl"];
        }

        public  async Task<IActionResult> Index()
        {
            var url = $"{apiurl}api/Karyawan/GetKaryawan";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Karyawan>>(content);
                ViewBag.datas = data;
            }
            else
            {
                ViewBag.datas = new List<Karyawan>();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
