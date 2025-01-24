using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [CheckAccess]
        public IActionResult MyProfile()
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
                    var UserData = new UserModel
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = "Customer",
                        IsActive = true,
                    };
                    var json = JsonConvert.SerializeObject(UserData);
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
                    ViewBag.Message = "Success";
                    ViewBag.Result = result.message;
                    return View("Login");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    ViewBag.Message = "Error";
                    ViewBag.Result = result.message;
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Message = "Failed";
                return View("Login");
            }
        }
        #endregion

        #region SaveLogin
        public async Task<IActionResult> SaveLogin(string NameOREmail, string password)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage res = null;
                try
                {
                    var UserData = new UserModel
                    {
                        UserName = NameOREmail,
                        Password = password,
                    };
                    var json = JsonConvert.SerializeObject(UserData);
                    var content = new StringContent(json,Encoding.UTF8, "application/json");
                    res = await _httpClient.PostAsync("User/Login",content);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if(res != null && res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserModel>(data);
                    if (user.IsActive == true)
                    {
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        HttpContext.Session.SetString("UserName", user.UserName);
                        HttpContext.Session.SetString("Email", user.Email);
                    }
                    else
                    {
                        var UserData = new UserModel
                        {
                            UserName = user.UserName,
                            Password = user.Password,
                            Email = user.Email,
                            Role = user.Role,
                            IsActive = true,
                        };
                        var json = JsonConvert.SerializeObject(UserData);
                        var content = new StringContent(json,Encoding.UTF8, "application/json");
                        res = await _httpClient.PutAsync($"User/UpdateUser/{user.UserId}",content);
                        if (res != null && res.IsSuccessStatusCode)
                        {
                            HttpContext.Session.SetString("UserId", user.UserId.ToString());
                            HttpContext.Session.SetString("UserName", user.UserName);
                            HttpContext.Session.SetString("Email", user.Email);
                        }
                    }
                    return RedirectToAction("MyProfile");
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    ViewBag.Message = "Error";
                    ViewBag.Result = result.message;
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Message = "Failed";
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
