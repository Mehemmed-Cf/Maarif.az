using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
<<<<<<< HEAD
    [Authorize(Policy = "RequireSuperAdminRole")]
=======
    //[Authorize(Roles = "SUPERADMIN")]
    [AllowAnonymous]
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
    public abstract class AdminBaseController : Controller
    {

    }
}
