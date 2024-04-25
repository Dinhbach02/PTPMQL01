using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MvcBach.Models;
namespace MvcBach.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
    return View();
        }
    
   [HttpPost]
   public IActionResult Index(Person ps )
   {
    string strOutput = " Xinchao " + ps.PersonId + "-" + ps.FullName + "-" + ps.Address;
    ViewBag.infoPerson = strOutput;
    return View();
   }
}
}

