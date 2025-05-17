using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class SpecificationController : Controller
    {
        private HttpClient _httpClient;
        private readonly string wwwrootDirBro = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\doc\\");

        #region configuration
        public SpecificationController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region GetSpecificationTV
        [HttpGet("/GetSpecificationTV/{id}")]
        public async Task<JsonResult> GetSpecificationTV(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync($"Specification/GetAllTV/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<SpecificationModel>(content);
                return Json(data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                return Json(data);
            }
        }
        #endregion

        #region GetSpecificationAC
        [HttpGet("/GetSpecificationAC/{id}")]
        public async Task<JsonResult> GetSpecificationAC(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync($"Specification/GetAllAC/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<SpecificationModel>(content);
                return Json(data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                return Json(data);
            }
        }
        #endregion

        #region AddSpecification
        [HttpPost]
        public async Task<IActionResult> AddSpecification(IFormCollection spec,IFormFile BUrl)
        {
            HttpResponseMessage res = null;
            SpecificationModel specification = new SpecificationModel();
            if (BUrl != null)
            {
                var path = Path.Combine(wwwrootDirBro, BUrl.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await BUrl.CopyToAsync(stream);
                }
                string url = "doc/" + BUrl.FileName;
                specification.BrochureUrl = url;
            }
            if (spec["type"] == "AC")
            {
                specification.ModelNumber = spec["ModelNumber"];
                specification.Brand = spec["Brand"];
                specification.Capacity = Convert.ToDouble(spec["Capacity"]);
                specification.Refrigerant = spec["Refrigerant"];
                specification.Voltage = Convert.ToInt32(spec["Voltage"]);
                specification.Color = spec["Color"];
                specification.Warranty = Convert.ToInt32(spec["Warranty"]);
                specification.StarRating = Convert.ToInt32(spec["StarRating"]);
                specification.ProductId = Convert.ToInt32(spec["ProductId"]);

                var json = JsonConvert.SerializeObject(specification);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                res = await _httpClient.PostAsync("Specification/PostAC", content);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return RedirectToAction("Index", "Product");
                }
            }
            else if(spec["type"] == "TV")
            {
                var json = JsonConvert.SerializeObject(specification);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                res = await _httpClient.PostAsync("Specification/PostTV", content);
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return RedirectToAction("Index", "Product"  );
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return RedirectToAction("Index", "Product");
                }
            }
            return View("ViewProduct");
        }
        #endregion
    }
}
