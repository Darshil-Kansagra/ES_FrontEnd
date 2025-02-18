using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Controllers
{
    [CheckAccess]
    public class OrderDetailsController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public OrderDetailsController(HttpClient http)
        {
            _httpClient = http;
        }
        #endregion

        #region GetAllOrderDetails
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("OrderDetails/GetAllOrderDetails");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(content);
                return View("OrderDetailsList", data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                Console.WriteLine(data);
                TempData["message"] = data.message;
                return View("OrderDetailsList");
            }
        }
        #endregion

        #region DeleteOrderDetails
        [HttpDelete("/DeleteOrderDetails/{id}")]
        public async Task<JsonResult> DeleteOrderDetails(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"OrderDetails/DeleteOrderDetails/{id}");
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
        public async Task<bool> SaveOrderDetails(OrderDetailsModel OrderDetails)
        {
            HttpResponseMessage res;
            var json = JsonConvert.SerializeObject(OrderDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            res = await _httpClient.PostAsync("OrderDetails/PostOrderDetails", content);
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
