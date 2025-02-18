using ES_FrontEnd.Areas.Admin.Controllers;
using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ES_FrontEnd.Controllers
{
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

        #region GetOrderbyUserid
        public async Task<List<OrderModel>> GetOrderbyUserId()
        {
            List<OrderModel> data = new List<OrderModel>();
            int? id = CommonVariable.UserId();
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync($"Order/GetByUserId/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                data = JsonConvert.DeserializeObject<List<OrderModel>>(content);
            }
            return data;
        }
        #endregion

        #region ViewPage
        public IActionResult Index()
        {
            return View("OrderPage");
        }
        #endregion

        #region checkout
        [HttpPost]
        [Route("/Checkout")]
        public async Task<IActionResult> Checkout(IFormCollection fc)
        {
            HttpResponseMessage res = null;
            try
            {
                res = await _httpClient.GetAsync($"Product/GetById/{fc["id"]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProductModel>(content);
                ViewBag.Product = data;
                TempData["Quantity"] = fc["Quantity"];
                int discount = 0;
                TempData["Discount"] = discount;
            }
            return View("Checkout");
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> SaveOrder(OrderModel model,IFormCollection fc)
        {
            HttpResponseMessage res = null;
            bool flag = false;
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                res = await _httpClient.PostAsync($"Order/PostOrder",content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (res != null && res.IsSuccessStatusCode)
            {
                List<OrderModel> values = new List<OrderModel>();
                values  = await GetOrderbyUserId();
                OrderDetailsModel odm = new OrderDetailsModel
                {
                    ProductId = Convert.ToInt32(fc["ProductId"]),
                    OrderId = values[0].OrderId ?? 0,
                    Quantity = Convert.ToInt32(fc["Quantity"]),
                    Amount = model.Price,
                    TotalAmount = (model.Price * Convert.ToInt32(fc["Quantity"])),
                    UserId = model.UserId
                }; 
                OrderDetailsController od1 = new OrderDetailsController(_httpClient);
                flag = await od1.SaveOrderDetails(odm);

                if(flag == true)
                {
                    BillModel bm = new BillModel()
                    {
                        Amount = model.Price,
                        Discount = Convert.ToInt32(fc["Discount"]),
                        TotalAmount = (model.Price * Convert.ToInt32(fc["Quantity"])),
                        OrderId = values[0].OrderId ?? 0,
                        UserId = model.UserId
                    };
                    BillController bc1 = new BillController(_httpClient);
                    flag = await bc1.SaveBills(bm);
                }
                return View("OrderSuccess");
            }
            else
            {
                TempData["message"] = "Sorry Your Order Can't be Taken";
                return View("Checkout");
            }
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
