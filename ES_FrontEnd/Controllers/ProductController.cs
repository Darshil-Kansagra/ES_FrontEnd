using Microsoft.AspNetCore.Mvc;

namespace ES_FrontEnd.Controllers
{
    [CheckAccess]
    public class ProductController : Controller
    {
        #region ViewPage
        public IActionResult Index()
        {
            return View("ProductPage");
        } 
        #endregion
    }
}
