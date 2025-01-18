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
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult SaveForm(ContactModel model)
        {
            if(ModelState.IsValid)
            {
                ViewBag.Message = "Success";
                return View("Contact");
            }
            else
            {
                ViewBag.Message = "Failed";
                return View("Contact");
            }
        }
    }
}
