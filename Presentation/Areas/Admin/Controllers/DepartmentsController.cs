using Application.Modules.DepartmentsModule.Commands.DepartmentsAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsRemoveCommand;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Repositories;
using Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    public class DepartmentsController : AdminBaseController
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IFacultyRepository facultyRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IGroupRepository groupRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMediator mediator;

        public DepartmentsController(IDepartmentRepository departmentRepository, IFacultyRepository facultyRepository, ITeacherRepository teacherRepository, IStudentRepository studentRepository, IGroupRepository groupRepository, ISubjectRepository subjectRepository, IMediator mediator)
        {
            this.departmentRepository = departmentRepository;
            this.facultyRepository = facultyRepository;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.groupRepository = groupRepository;
            this.subjectRepository = subjectRepository;
            this.mediator = mediator;
        }

        private void PopulateViewBags(int? selectedId = null)
        {
            // Fill Faculties
            var faculties = facultyRepository.GetAll()?.ToList() ?? new List<Faculty>();
            ViewBag.Faculties = new SelectList(faculties, "Id", "Name", selectedId);

            // Fill the others! Even if they are empty lists for now, 
            // they MUST NOT be null.
            ViewBag.Teachers = new SelectList(teacherRepository.GetAll()?.ToList() ?? new List<Teacher>(), "Id", "FullName");
            ViewBag.Students = new SelectList(studentRepository.GetAll()?.ToList() ?? new List<Student>(), "Id", "FullName");
            ViewBag.Groups = new SelectList(groupRepository.GetAll()?.ToList() ?? new List<Group>(), "Id", "Name");
            ViewBag.Subjects = new SelectList(subjectRepository.GetAll()?.ToList() ?? new List<Subject>(), "Id", "Name");
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new DepartmentGetAllRequest{});
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] DepartmentGetByIdRequest request)
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
        public async Task<IActionResult> Create([FromForm] DepartmentAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBags(request.FacultyId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit([FromRoute] DepartmentGetByIdRequest request)
        {
            //PopulateViewBags();

            var response = await mediator.Send(request);

            PopulateViewBags(response.FacultyId);


            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] DepartmentEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Re-fill the dropdown so the user can try again
                PopulateViewBags(request.FacultyId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove([FromRoute] DepartmentRemoveRequest request)
        {
            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
