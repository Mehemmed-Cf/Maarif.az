using Application.Modules.SubjectsModule.Commands.SubjectAddCommand;
using Application.Modules.SubjectsModule.Commands.SubjectEditCommand;
using Application.Modules.SubjectsModule.Commands.SubjectRemoveCommand;
using Application.Modules.SubjectsModule.Queries.SubjectGetAllQuery;
using Application.Modules.SubjectsModule.Queries.SubjectGetByIdQuery;
using Application.Modules.SubjectsModule.Queries.SubjectGetForEditQuery;
using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    public class SubjectsController : AdminBaseController
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
            var departments = departmentRepository.GetAll()?.ToList() ?? new List<Department>();
            ViewBag.Departments = new SelectList(departments, "Id", "Name", selectedId);
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new SubjectGetAllRequest { });
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] SubjectGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            PopulateViewBags();
            return View(new SubjectAddRequest());
        }

        [HttpPost]
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

        public async Task<IActionResult> Edit([FromRoute] SubjectGetByIdRequest request)
        {
            var form = await mediator.Send(new SubjectGetForEditRequest { Id = request.Id });
            PopulateViewBags(form.DepartmentId);
            return View(form);
        }

        [HttpPost]
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
        public async Task<IActionResult> Remove([FromRoute] SubjectRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
