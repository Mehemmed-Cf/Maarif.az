using Application.Modules.FacultiesModule.Commands.FacultyAddCommand;
using Application.Modules.FacultiesModule.Commands.FacultyEditCommand;
using Application.Modules.FacultiesModule.Commands.FacultyRemoveCommand;
using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    public class FacultiesController : AdminBaseController
    {
        private readonly IFacultyRepository facultyRepository;
        private readonly IMediator mediator;

        public FacultiesController(IFacultyRepository facultyRepository, IMediator mediator)
        {
            this.facultyRepository = facultyRepository;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new FacultyGetAllRequest { });
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] FacultyGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FacultyAddRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] FacultyGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] FacultyEditRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] FacultyRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
