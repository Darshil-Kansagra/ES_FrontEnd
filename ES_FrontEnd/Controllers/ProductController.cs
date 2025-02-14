using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ES_FrontEnd.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public ProductController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region ProductList
        [HttpPost]
        public async Task<IActionResult> Index(SearchProductModel model)
        {
            HttpResponseMessage res = null;
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                res = await _httpClient.PostAsync("Product/SearchProduct",content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                ViewBag.Products = data;
                return View("ProductPage");
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                Console.WriteLine(data.message);
                return View("ProductPage");
            }
        }
        #endregion

        #region ProductSingle
        public async Task<IActionResult> ProductSingle(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync($"Product/GetById/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductModel>(content);
                ViewBag.Product = data;
                return View("ProductSingle");
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("ProductSingle");
            }
        }
        #endregion
    }
}
