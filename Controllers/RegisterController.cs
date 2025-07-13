using Microsoft.AspNetCore.Mvc;

namespace ThemeConversion.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}
