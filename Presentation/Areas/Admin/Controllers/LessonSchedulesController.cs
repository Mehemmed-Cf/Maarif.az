using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleAddCommand;
using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleEditCommand;
using Application.Modules.LessonSchedulesModule.Commands.LessonScheduleRemoveCommand;
using Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetAllQuery;
using Application.Modules.LessonSchedulesModule.Queries.LessonScheduleGetByIdQuery;
using Application.Repositories;
using Domain.Models.Stables;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonSchedulesController : AdminBaseController
    {
        private readonly IMediator mediator;
        private readonly ILessonScheduleRepository lessonScheduleRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IRoomRepository roomRepository;

        public LessonSchedulesController(
            IMediator mediator,
            ILessonScheduleRepository lessonScheduleRepository,
            ILessonRepository lessonRepository,
            IGroupRepository groupRepository,
            IRoomRepository roomRepository)
        {
            this.mediator = mediator;
            this.lessonScheduleRepository = lessonScheduleRepository;
            this.lessonRepository = lessonRepository;
            this.groupRepository = groupRepository;
            this.roomRepository = roomRepository;
        }

        private async Task PopulateViewBagsAsync(int? selectedLessonId = null, int? selectedGroupId = null)
        {
            var lessons = await Task.Run(() => lessonRepository.GetAll().Select(l => new 
            { 
                Id = l.Id, 
                Name = l.Subject.Name + " - " + l.Teacher.FullName 
            }).ToList());
            ViewBag.Lessons = new SelectList(lessons, "Id", "Name", selectedLessonId);

            var groups = await Task.Run(() => groupRepository.GetAll().Select(g => new
            {
                Id = g.Id,
                Name = g.Name
            }).ToList());
            ViewBag.Groups = new SelectList(groups, "Id", "Name", selectedGroupId);

            var rooms = await Task.Run(() => roomRepository.GetAll()
                .Include(r => r.Building)
                .ToList());

            ViewBag.Rooms = new SelectList(
                rooms.Select(r => new { r.Id, Display = $"{r.Building.Id}-{r.Number}" }),
                "Id", "Display");

            ViewBag.DayOfWeek = new SelectList(Enum.GetValues<DayOfWeek>());
            ViewBag.LessonType = new SelectList(Enum.GetValues<LessonType>());
            ViewBag.WeekType = new SelectList(Enum.GetValues<WeekType>());
        }

        public async Task<IActionResult> Index()
        {
            var response = await mediator.Send(new LessonScheduleGetAllRequest());
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] LessonScheduleGetByIdRequest request)
        {
            var response = await mediator.Send(request);
            return View(response);
        }

        public async Task<IActionResult> Create(int? lessonId = null)
        {
            ViewBag.PreselectedLessonId = lessonId;
            await PopulateViewBagsAsync(selectedLessonId: lessonId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonScheduleAddRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.LessonId, request.GroupId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await mediator.Send(new LessonScheduleGetByIdRequest { Id = id });

            var editRequest = new LessonScheduleEditRequest
            {
                Id = response.Id,
                LessonId = response.LessonId,
                GroupId = response.GroupId,
                DayOfWeek = response.DayOfWeek,
                StartTime = response.StartTime,
                EndTime = response.EndTime,
                RoomId = response.RoomId,
                LessonType = response.LessonType,
                WeekType = response.WeekType
            };

            await PopulateViewBagsAsync(response.LessonId, response.GroupId);

            return View(editRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LessonScheduleEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagsAsync(request.LessonId, request.GroupId);
                return View(request);
            }

            await mediator.Send(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await mediator.Send(new LessonScheduleRemoveRequest { Id = id });
            return Ok(new { message = "Lesson schedule successfully deleted" });
        }
    }
}