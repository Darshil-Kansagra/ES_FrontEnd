using System.Diagnostics;
using ES_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace ES_FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region ContactViewPage
        [Route("Contact")]
        public IActionResult ContactPage()
        {
            return View();
        }
        #endregion

        #region SaveContactData
        public IActionResult SaveForm(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Success";
                return View("ContactPage");
            }
            else
            {
                ViewBag.Message = "Failed";
                return View("ContactPage");
            }
        }
        #endregion
    }
}
