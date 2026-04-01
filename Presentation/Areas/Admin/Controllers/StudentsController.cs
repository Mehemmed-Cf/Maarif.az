using Application.Modules.StudentsModule.Commands.StudentAddCommand;
using Application.Modules.StudentsModule.Commands.StudentEditCommand;
using Application.Modules.StudentsModule.Commands.StudentRemoveCommand;
using Application.Modules.StudentsModule.Queries.StudentGetAllQuery;
using Application.Modules.StudentsModule.Queries.StudentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    //[Authorize(Roles = "SUPERADMIN", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Area("admin")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public StudentsController(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IGroupRepository groupRepository,
            IMapper mapper,
            IMediator mediator)
        {
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.groupRepository = groupRepository;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        private void PopulateViewBags(int? selectedDepartmentId = null)
        {
            ViewBag.Departments = new SelectList(
                departmentRepository.GetAll()?.ToList() ?? new List<Department>(),
                "Id", "Name", selectedDepartmentId);

            ViewBag.Groups = new SelectList(
                groupRepository.GetAll()?.ToList() ?? new List<Group>(),
                "Id", "Name");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new StudentGetAllRequest { });
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] StudentGetByIdRequest request)
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
        public async Task<IActionResult> Create([FromForm] StudentAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet] // Fixes AmbiguousMatchException
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromRoute] StudentGetByIdRequest request)
        {
            PopulateViewBags();
            var response = await mediator.Send(request);

            // View expects StudentGetByIdResponseDto
            PopulateViewBags(response.Department.Id);
            return View(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromForm] StudentEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.DepartmentId);

                var model = mapper.Map<StudentGetByIdResponseDto>(request);

                return View(model);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute] StudentRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
