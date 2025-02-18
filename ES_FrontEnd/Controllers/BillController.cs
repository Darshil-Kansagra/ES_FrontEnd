using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Controllers
{
    [CheckAccess]
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
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("Bill/GetAllBill");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<BillModel>>(content);
                return View("BillList", data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("BillList");
            }
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
        public async Task<bool> SaveBills(BillModel Bill)
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
    }
    #endregion
}
