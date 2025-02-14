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
    public class OrderDetailsController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public OrderDetailsController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
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

        #region OrderDetails Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"OrderDetails/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OrderDetailsModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(OrderDetailsModel OrderDetails)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(OrderDetails);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res;
                if (OrderDetails.OrderDetailId == null)
                {
                    res = await _httpClient.PostAsync("OrderDetails/PostOrderDetails", content);
                }
                else
                {
                    res = await _httpClient.PutAsync($"OrderDetails/PutOrderDetails/{OrderDetails.OrderDetailId}", content);
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
                    return RedirectToAction("Insert", "OrderDetails");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return RedirectToAction("Insert", "OrderDetails");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert", "OrderDetails");
            }
        }
        #endregion

        #region GetUserDropDown
        [HttpGet]
        [Route("/UserDropDown")]
        public async Task<JsonResult> GetUserDropDown()
        {
            HttpResponseMessage res = null;
            res = await _httpClient.GetAsync("User/GetAllUser");
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserDropDown>>(content);
                Console.WriteLine(data);
                ViewBag.UserList = data;
                return Json(data);
            }
            return Json(null);
        }
        #endregion
    }
}
