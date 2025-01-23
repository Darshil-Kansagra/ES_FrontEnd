using Microsoft.AspNetCore.Mvc;

namespace ES_FrontEnd.Controllers
{
    public class UserController : Controller
    {
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
