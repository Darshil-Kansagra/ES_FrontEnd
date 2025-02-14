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
    public class BillController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public BillController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
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

        #region Bill Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"Bill/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BillModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(BillModel Bill)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Bill);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res;
                if (Bill.BillId == null)
                {
                    res = await _httpClient.PostAsync("Bill/PostBill", content);
                }
                else
                {
                    res = await _httpClient.PutAsync($"Bill/PutBill/{Bill.BillId}", content);
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
                    return RedirectToAction("Insert", "Bill");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return RedirectToAction("Insert", "Bill");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert", "Bill");
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
