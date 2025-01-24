using Microsoft.AspNetCore.Mvc;

namespace ES_FrontEnd.Controllers
{
    [CheckAccess]
    public class OrderController : Controller
    {
        #region ViewPage
        public IActionResult Index()
        {
            return View("OrderPage");
        } 
        #endregion
    }
}
