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
    public class UserController : Controller
    {
        private HttpClient _httpClient;

        #region configuration
        public UserController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region GetAllUser
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync("User/GetAllUser");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if(res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<UserModel>>(content);
                return View("UserList",data);
            }
            else
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                TempData["message"] = data.message;
                return View("UserList");
            }
        }
        #endregion

        #region SoftDeleteUser
        [HttpDelete("/SoftDelete/{id}")]
        public async Task<JsonResult> SoftDeleteUser(int id)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.DeleteAsync($"User/SoftDelete/{id}");
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

        #region User Insert
        [Route("Insert")]
        public async Task<IActionResult> Insert(int? id)
        {
            if (id.HasValue)
            {
                var res = await _httpClient.GetAsync($"User/GetById/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UserModel>(data);
                    return View(result);
                }
            }
            return View("Insert");
        }
        #endregion

        #region Save
        [HttpPost]
        public async Task<IActionResult> Save(UserModel User)
        {
            if (ModelState.IsValid)
            {
                User.IsActive = true;
                var json = JsonConvert.SerializeObject(User);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage res;
                if (User.UserId == null)
                {
                    res = await _httpClient.PostAsync("User/PostUser", content);
                }
                else
                {
                    res = await _httpClient.PutAsync($"User/UpdateUser/{User.UserId}", content);
                }
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","User");
                }
                else if (res.StatusCode == HttpStatusCode.BadRequest)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    Console.WriteLine(result);
                    TempData["message"] = "Please Fill All Field in the Form";
                    return RedirectToAction("Insert","User");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return RedirectToAction("Insert","User");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return RedirectToAction("Insert");
            }
        }
        #endregion
    }
}

