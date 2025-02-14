using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [CheckAccess]
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

        #region AddSpecificationAC
        [HttpPost]
        [Route("SaveSpecificationAC")]
        public async Task<JsonResult> AddSpecificationAC(SpecificationModel specification)
        {
            HttpResponseMessage res = null;
            var json = JsonConvert.SerializeObject(specification);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            res = await _httpClient.PostAsync("Specification/PostAC", content);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return Json(new { message = Convert.ToString(result.message)});
            }
            else
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return Json(new { message = Convert.ToString(result.message)});
            }
        }
        #endregion

        #region AddSpecificationTV
        [HttpPost]
        [Route("SaveSpecificationTV")]
        public async Task<JsonResult> AddSpecificationTV(SpecificationModel specification)
        {
            HttpResponseMessage res = null;
            var json = JsonConvert.SerializeObject(specification);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            res = await _httpClient.PostAsync("Specification/PostTV", content);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return Json(new { message = Convert.ToString(result.message) });
            }
            else
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return Json(new { message = Convert.ToString(result.message) });
            }
        }
        #endregion
    }
}
