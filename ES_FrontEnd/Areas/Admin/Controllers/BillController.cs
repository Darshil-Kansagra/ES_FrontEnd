using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class BillController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public BillController(HttpClient http)
        {
            _httpClient = http;
        }
        #endregion

        #region GetAllBill
        public async Task<List<BillModel>> GetAllBill(int id)
        {
            HttpResponseMessage res = null;
            List<BillModel> data = new List<BillModel>();
            try
            {
                res = await _httpClient.GetAsync($"Bill/GetAllBill/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<BillModel>>(content);
            }
            return data;
        }
        #endregion

        #region DeleteBill
        [HttpDelete("/DeleteBill/{id}")]
        public async Task<JsonResult> DeleteBill(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"Bill/DeleteBill/{id}");
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
        public async Task<bool> Save(BillModel Bill)
        {
            HttpResponseMessage res;
            var json = JsonConvert.SerializeObject(Bill);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            res = await _httpClient.PostAsync("Bill/PostBill", content);
            if (res.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
