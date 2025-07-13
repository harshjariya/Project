using Microsoft.AspNetCore.Mvc;

namespace QuizManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
