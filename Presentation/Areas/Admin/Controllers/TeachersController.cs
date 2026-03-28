using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SUPERADMIN", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Area("admin")]
    public class TeachersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
