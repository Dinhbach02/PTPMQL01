using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
namespace MvcBach.Controllers
{
    public class HelloWorldController : Controller
    { 
      
        public IActionResult Index()
        {
            return View();
            
        } 
       


        public string Welcome()
        {
            return "..Tran Dinh Bach 02.";
        }
    }
}
