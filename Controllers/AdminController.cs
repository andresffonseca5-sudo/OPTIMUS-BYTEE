using Microsoft.AspNetCore.Mvc;

namespace OPTIMUS_BYTEE.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}