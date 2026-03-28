using Application.Modules.SubjectsModule.Commands.SubjectAddCommand;
using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand;
using Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery;
using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SUPERADMIN", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Area("admin")]
    public class SubjectsController : Controller
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMediator mediator;

        public SubjectsController(ISubjectRepository subjectRepository, IDepartmentRepository departmentRepository, IMediator mediator)
        {
            this.subjectRepository = subjectRepository;
            this.departmentRepository = departmentRepository;
            this.mediator = mediator;
        }

        private void PopulateViewBags(int? selectedId = null)
        {
            // For Subjects, we need a list of Departments to choose from
            var departments = departmentRepository.GetAll()?.ToList() ?? new List<Department>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name", selectedId);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new SubjectGetAllRequest { });
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] SubjectGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] SubjectAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }


        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromRoute] SubjectGetByIdRequest request)
        {
            PopulateViewBags();

            var response = await mediator.Send(request);

            PopulateViewBags(response.Department.Id);


            return View(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromForm] SubjectEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Re-fill the dropdown so the user can try again
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute] SubjectRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
