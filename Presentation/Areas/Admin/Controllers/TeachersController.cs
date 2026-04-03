using Application.Modules.TeachersModule.Commands.TeacherAddCommand;
using Application.Modules.TeachersModule.Commands.TeacherEditCommand;
using Application.Modules.TeachersModule.Commands.TeacherRemoveCommand;
using Application.Modules.TeachersModule.Queries.TeacherGetAllQuery;
using Application.Modules.TeachersModule.Queries.TeacherGetByIdQuery;
using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;

namespace Presentation.Areas.Admin.Controllers
{
    public class TeachersController : AdminBaseController
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMediator mediator;

        public TeachersController(
            ITeacherRepository teacherRepository,
            IDepartmentRepository departmentRepository,
            ISubjectRepository subjectRepository,
            IGroupRepository groupRepository,
            ILessonRepository lessonRepository,
            IMediator mediator)
        {
            this.teacherRepository = teacherRepository;
            this.departmentRepository = departmentRepository;
            this.subjectRepository = subjectRepository;
            this.groupRepository = groupRepository;
            this.lessonRepository = lessonRepository;
            this.mediator = mediator;
        }

        private void PopulateViewBags(IEnumerable<int> selectedDepartmentIds = null)
        {
            var departments = departmentRepository.GetAll()?.ToList() ?? new List<Department>();

            ViewBag.Departments = new MultiSelectList(
                departments,
                "Id",
                "Name",
                selectedDepartmentIds
            );

            ViewBag.Subjects = new SelectList(
                subjectRepository.GetAll()?.ToList() ?? new List<Subject>(),
                "Id", "Name");

            ViewBag.Groups = new SelectList(
                groupRepository.GetAll()?.ToList() ?? new List<Group>(),
                "Id", "Name");

            ViewBag.Lessons = new SelectList(
                lessonRepository.GetAll()?.ToList() ?? new List<Lesson>(),
                "Id", "Id");
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new TeacherGetAllRequest { });
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] TeacherGetByIdRequest request)
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
        public async Task<IActionResult> Create([FromForm] TeacherAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.DepartmentIds);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] TeacherGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            PopulateViewBags();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] TeacherEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags();
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] TeacherRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
