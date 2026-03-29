using Application.Modules.GroupsModule.Commands.GroupAddCommand;
using Application.Modules.GroupsModule.Commands.GroupEditCommand;
using Application.Modules.GroupsModule.Commands.GroupRemoveCommand;
using Application.Modules.GroupsModule.Queries.GroupGetAllQuery;
using Application.Modules.GroupsModule.Queries.GroupGetByIdQuery;
using Application.Modules.LessonsModule;
using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("admin")]
    public class GroupsController : Controller
    {
        private readonly IGroupRepository groupRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMediator mediator;

        public GroupsController(
            IGroupRepository groupRepository,
            IDepartmentRepository departmentRepository,
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            IMediator mediator)
        {
            this.groupRepository = groupRepository;
            this.departmentRepository = departmentRepository;
            this.studentRepository = studentRepository;
            this.lessonRepository = lessonRepository;
            this.mediator = mediator;
        }

        private async Task PopulateViewBagsAsync(int? selectedDepartmentId = null)
        {
            var departments = departmentRepository.GetAll()?.ToList() ?? new List<Department>();
            var students = studentRepository.GetAll()?.ToList() ?? new List<Student>();
            var lessons = (await lessonRepository.GetAllAsync())?.ToList() ?? new List<LessonResponseDto>();

            ViewBag.Departments = new SelectList(departments, "Id", "Name", selectedDepartmentId);
            ViewBag.Students = new SelectList(students, "Id", "FullName");
            ViewBag.Lessons = new SelectList(
                lessons.Select(l => new {
                    l.Id,
                    DisplayName = $"{l.SubjectName} - {l.TeacherFullName}"
                }),
                "Id",
                "DisplayName"
            );
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new GroupGetAllRequest());
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details([FromRoute] GroupGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            await PopulateViewBagsAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] GroupAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.DepartmentId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromRoute] GroupGetByIdRequest request)
        {
            var response = await mediator.Send(request);

            // Safety check: if group doesn't exist, don't try to show the view
            if (response == null) return NotFound();

            await PopulateViewBagsAsync(response.DepartmentId);
            return View(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromForm] GroupEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.DepartmentId);
                //return View(request);
                return RedirectToAction(nameof(Edit), new { id = request.Id });
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Remove([FromRoute] GroupRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}