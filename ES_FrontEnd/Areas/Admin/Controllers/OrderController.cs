using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ES_FrontEnd.Areas.Admin.Models;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
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

        #region ViewOrder
        [HttpGet("{id}")]
        public async Task<IActionResult> ViewOrder(int id)
        {
            OrderDetailsController od = new OrderDetailsController(_httpClient);
            List<OrderDetailsModel> odm = new List<OrderDetailsModel>();
            odm = await od.GetAllOrderDetails(id);
            ViewBag.OrderDetail = odm;

            BillController bc = new BillController(_httpClient);
            List<BillModel> bm = new List<BillModel>();
            bm = await bc.GetAllBill(id);
            ViewBag.Bills = bm;
            return View("ViewOrder");
        }
        #endregion

        #region SaveBills
        [HttpPost]
        [Route("/SaveBills")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous] // Add this attribute to bypass CheckAccess
        public async Task<bool> SaveBills(BillModel Bill)
        {
            BillController bc = new BillController(_httpClient);
            return await bc.Save(Bill);
        }
        #endregion
    }
}