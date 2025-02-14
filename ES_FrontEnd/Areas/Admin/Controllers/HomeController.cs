using Microsoft.AspNetCore.Mvc;

namespace ES_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [CheckAccess]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
