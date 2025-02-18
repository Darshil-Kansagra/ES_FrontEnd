using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ES_FrontEnd.Areas.Admin.Models;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [CheckAccess]
    public class OrderController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public OrderController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region GetAllOrder
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("Order/GetAllOrder");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<OrderModel>>(content);
                return View("OrderList", data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("OrderList");
            }
        }
        #endregion

        #region DeleteOrder
        [HttpDelete("/DeleteOrder/{id}")]
        public async Task<JsonResult> DeleteOrder(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"Order/DeleteOrder/{id}");
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

        #region Order Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"Order/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OrderModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(OrderModel Order)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res;
                if (Order.OrderId == null)
                {
                    res = await _httpClient.PostAsync("Order/PostOrder", content);
                }
                else
                {
                    res = await _httpClient.PutAsync($"Order/PutOrder/{Order.OrderId}", content);
                }
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (res.StatusCode == HttpStatusCode.BadRequest)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    Console.WriteLine(result);
                    TempData["message"] = "Please Fill All Field in the Form";
                    return RedirectToAction("Insert", "Order");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return RedirectToAction("Insert", "Order");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert", "Order");
            }
        }
        #endregion

    }
}
