using ES_FrontEnd.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [CheckAccess]
    public class CustomerController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public CustomerController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region GetAllCustomer
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("Customer/GetAllCustomer");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<CustomerModel>>(content);
                return View("CustomerList", data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("CustomerLists");
            }
        }
        #endregion

        #region DeleteCustomer
        [HttpDelete("/DeleteCustomer/{id}")]
        public async Task<JsonResult> DeleteCustomer(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"Customer/DeleteCustomer/{id}");
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

        #region Customer Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"Customer/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<CustomerModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(CustomerModel Customer)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(Customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res;
                if (Customer.CustomerId == null)
                {
                    res = await _httpClient.PostAsync("Customer/PostCustomer", content);
                }
                else
                {
                    res = await _httpClient.PutAsync($"Customer/PutCustomer/{Customer.CustomerId}", content);
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
                    return RedirectToAction("Insert", "Customer");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return RedirectToAction("Insert", "Customer");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert", "Customer");
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
