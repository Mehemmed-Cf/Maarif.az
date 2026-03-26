using Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SUPERADMIN", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Area("admin")]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMediator mediator;

        public DepartmentsController(IDepartmentRepository departmentRepository, IMediator mediator)
        {
            this.departmentRepository = departmentRepository;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new DepartmentGetAllRequest{});
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] DepartmentGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] DepartmentAddRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromRoute] DepartmentGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromForm] DepartmentEditRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute] DepartmentRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
