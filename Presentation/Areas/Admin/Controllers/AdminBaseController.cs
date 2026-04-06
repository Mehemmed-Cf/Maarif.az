using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireSuperAdminRole")]
    public abstract class AdminBaseController : Controller
    {

    }
}
