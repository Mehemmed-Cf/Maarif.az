using Application.Modules.StudentsModule.Commands.StudentAddCommand;
using Application.Modules.StudentsModule.Commands.StudentEditCommand;
using Application.Modules.StudentsModule.Commands.StudentRemoveCommand;
using Application.Modules.StudentsModule.Queries.StudentGetAllQuery;
using Application.Modules.StudentsModule.Queries.StudentGetByIdQuery;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    public class StudentsController : AdminBaseController
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

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new StudentGetAllRequest { });
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] StudentGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public IActionResult Create()
        {
            PopulateViewBags();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StudentAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }

            try
            {
                await mediator.Send(request);
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                PopulateViewBags(request.DepartmentId);
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] // Fixes AmbiguousMatchException
        public async Task<IActionResult> Edit([FromRoute] StudentGetByIdRequest request)
        {
            PopulateViewBags();
                var response = await mediator.Send(request);

            // View expects StudentGetByIdResponseDto
            PopulateViewBags(response.Department.Id);
            return View(response);
        }

        [HttpPost]
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
        public async Task<IActionResult> Remove([FromRoute] StudentRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
