using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ES_FrontEnd.Controllers
{
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

        #region MyProfile
        [Route("MyProfile")]
        public IActionResult MyProfile(dynamic? result)
        {
            return View();
        }
        #endregion

        #region LoginView
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region SaveSignUp
        [HttpPost]
        public async Task<IActionResult> SaveSignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = null;
                try
                {
                    model.IsActive = true;
                    model.Role = "Customer";
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    res = await _httpClient.PostAsync("User/PostUser", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (res != null && res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return View("Login");
                }
                else if(res != null && res.StatusCode == HttpStatusCode.BadRequest)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    Console.WriteLine(result);
                    TempData["message"] = "Please Fill All Field in the Form";
                    return View("Login");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return View("Login");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return View("Login");
            }
        }
        #endregion

        #region SaveLogin
        public async Task<IActionResult> SaveLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = null;
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json,Encoding.UTF8, "application/json");
                res = await _httpClient.PostAsync("User/Login",content);

                if(res != null && res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    if (user.IsActive == true)
                    {
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        HttpContext.Session.SetString("UserName", user.UserName);
                        HttpContext.Session.SetString("Email", user.Email);
                        HttpContext.Session.SetString("Role", user.Role);
                    }
                    else
                    {
                        user.IsActive = true;
                        json = JsonConvert.SerializeObject(user);
                        content = new StringContent(json,Encoding.UTF8, "application/json");
                        res = await _httpClient.PutAsync($"User/UpdateUser/{user.UserId}",content);
                        if (res != null && res.IsSuccessStatusCode)
                        {
                            HttpContext.Session.SetString("UserId", user.UserId.ToString());
                            HttpContext.Session.SetString("UserName", user.UserName);
                            HttpContext.Session.SetString("Email", user.Email);
                            HttpContext.Session.SetString("Role", user.Role);
                        }
                    }
                    if(user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("MyProfile");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return View("Login");
                }
            }
            else
            {
                TempData["message"] = "Please Fill All Field in the Form";
                return View("Login");
            }
        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion
    }
}
