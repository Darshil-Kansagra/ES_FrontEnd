using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [CheckAccess]
    public class ProductController : Controller
    {
        private readonly string wwwrootDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Uploads");
        private HttpClient _httpClient;

        #region configuration
        public ProductController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region GetAllProduct
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("Product/GetAllProduct");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<ProductModel>>(content);
                return View("ProductList", data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("ProductList");
            }
        }
        #endregion

        #region Product Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"Product/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ProductModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Delete Product
        [HttpDelete("/DeleteProduct/{id}")]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"Product/DeleteProduct/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(content);
                return Json(new { message = Convert.ToString(result.message) });
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(content);
                return Json(new { message = Convert.ToString(result.message) });
            }
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(ProductModel product,IFormFile image)
        {
            SpecificationModel specification = new SpecificationModel();
            if (image != null)
            {
                var path = Path.Combine(wwwrootDir,image.FileName);

                using(var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                string url = "Uploads/"+ image.FileName;
                product.ImageUrl = url;
            }
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage res;
            if (product.ProductId == null)
            {
                res = await _httpClient.PostAsync("Product/PostProduct", content);
            }
            else
            {
                res = await _httpClient.PutAsync($"Product/PutProduct/{product.ProductId}", content);
            }
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Product");
            }
            else if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                Console.WriteLine(result);
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert", "Product");
            }
            else
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                TempData["message"] = result.message;
                return RedirectToAction("Insert", "Product");
            }
        }
        #endregion

        #region ViewProduct
        [Route("ViewProduct")]
        [HttpPost]
        public async Task<IActionResult> ViewProduct(int id)
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
                return View(data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View();
            }
        }
        #endregion
    }
}
