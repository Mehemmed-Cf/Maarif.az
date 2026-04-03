using Application.Modules.LessonsModule.Commands.LessonAddCommand;
using Application.Modules.LessonsModule.Commands.LessonEditCommand;
using Application.Modules.LessonsModule.Commands.LessonRemoveCommand;
using Application.Modules.LessonsModule.Queries.LessonGetAllQuery;
using Application.Modules.LessonsModule.Queries.LessonGetByIdQuery;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Areas.Admin.Controllers
{
    public class LessonsController : AdminBaseController
    {
        private readonly ILessonRepository lessonRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMediator mediator;

        public LessonsController(ILessonRepository lessonRepository, ITeacherRepository teacherRepository, ISubjectRepository subjectRepository, IMediator mediator)
        {
            this.lessonRepository = lessonRepository;
            this.teacherRepository = teacherRepository;
            this.subjectRepository = subjectRepository;
            this.mediator = mediator;
        }

        private async Task PopulateViewBagsAsync(int? selectedSubjectId = null, int? selectedTeacherId = null)
        {
            // Subjects act as the bridge to Departments
            var subjects = await Task.Run(() => subjectRepository.GetAll().ToList());
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name", selectedSubjectId);

            // Teachers assigned to lessons
            var teachers = await Task.Run(() => teacherRepository.GetAll().ToList());
            ViewBag.Teachers = new SelectList(teachers, "Id", "FullName", selectedTeacherId);
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new LessonGetAllRequest { });
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] LessonGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateViewBagsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.SubjectId, request.TeacherId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await mediator.Send(new LessonGetByIdRequest { Id = id });

            await PopulateViewBagsAsync(response.SubjectId, response.TeacherId);

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LessonEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Re-fill the dropdowns so the page doesn't crash on reload
                await PopulateViewBagsAsync(request.SubjectId, request.TeacherId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await mediator.Send(new LessonRemoveRequest { Id = id });
            return Ok(new { message = "Lesson successfully deleted" });
        }
    }
}
