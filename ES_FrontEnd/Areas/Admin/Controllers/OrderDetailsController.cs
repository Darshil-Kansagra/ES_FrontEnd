using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
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
        public async Task<List<OrderDetailsModel>> GetAllOrderDetails(int id)
        {
            HttpResponseMessage res = null;
            List<OrderDetailsModel> data = new List<OrderDetailsModel>();
            try
            {
                res = await _httpClient.GetAsync($"OrderDetails/GetAllOrderDetails/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(content);
            }
            return data;
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
    }
}
