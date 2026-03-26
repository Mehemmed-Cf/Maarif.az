using Application.Modules.FacultiesModule.Commands.FacultyAddCommand;
using Application.Modules.FacultiesModule.Commands.FacultyEditCommand;
using Application.Modules.FacultiesModule.Commands.FacultyRemoveCommand;
using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{    //[Authorize(Roles = "SUPERADMIN", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Area("admin")]
    public class FacultiesController : Controller
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMediator mediator;

        public FacultiesController(IFacultyRepository facultyRepository, IMediator mediator)
        {
            this.facultyRepository = facultyRepository;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new FacultyGetAllRequest { });
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] FacultyGetByIdRequest request)
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
        public async Task<IActionResult> Create([FromForm] FacultyAddRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromRoute] FacultyGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromForm] FacultyEditRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute] FacultyRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
