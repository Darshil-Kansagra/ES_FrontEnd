using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace ES_FrontEnd.Controllers
{
    public class CustomerController : Controller
    {
        private static HttpClient _httpClient;

        #region configuration
        public CustomerController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(configuration["ApiBaseUrl"]);
        }
        #endregion

        #region getCustomer
        [HttpGet]
        public async Task<JsonResult> getCustomer()
        {
            int? id = CommonVariable.UserId();
            HttpResponseMessage res = null;
            res = await _httpClient.GetAsync($"Customer/GetByUserId/{id}");
            if (res != null && res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CustomerModel>(data);
                return Json(result);
            }
            else
            {
                var data = await res.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return Json(new { message = Convert.ToString(result.message) });
            }
        }
        #endregion

        #region SaveCustomer
        [HttpPost]
        public async Task<JsonResult> SaveCustomerAsync(CustomerModel customer)
        {
            if (customer.CustomerId == null)
            {
                HttpResponseMessage res = null;
                try
                {
                    var json = JsonConvert.SerializeObject(customer);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    res = await _httpClient.PostAsync("Customer/PostCustomer", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                if (res != null && res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return Json(new {message = Convert.ToString(result.message)});
                }
                else if(res != null && res.StatusCode == HttpStatusCode.BadRequest)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    Console.WriteLine(result);
                    return Json(new { message = "Please Fill All Field in the Form" });
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return Json(new { message = Convert.ToString(result.message) });
                }
            }
            else
            {
                HttpResponseMessage res = null;
                try
                {
                    var json = JsonConvert.SerializeObject(customer);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    res = await _httpClient.PutAsync($"Customer/PutCustomer/{customer.CustomerId}", content);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (res != null && res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    return Json(new { message = Convert.ToString(result.message) });
                }
                else if (res != null && res.StatusCode == HttpStatusCode.BadRequest)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    Console.WriteLine(result);
                    return Json(new { message = "Please Fill All Field in the Form" });
                }
                else
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(data);
                    TempData["message"] = result.message;
                    return Json(new { message = Convert.ToString(result.message) });
                }
            }
        }
        #endregion
    }
}
