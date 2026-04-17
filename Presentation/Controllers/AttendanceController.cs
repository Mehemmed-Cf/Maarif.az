using Application.Modules.AttendanceModule.Commands.TeacherMarkAttendanceCommand;
using Application.Modules.AttendanceModule.Queries.StudentAttendanceQuery;
using Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionDetailsQuery;
using Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionsQuery;
using Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery;
using Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery;
using Application.Modules.TeachersModule.Queries.GetTeacherScheduleQuery;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.AppCode;
using Presentation.AppCode.Extensions;
using Presentation.AppCode.ViewModels;

namespace Presentation.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly IMediator mediator;

        public AttendanceController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            await PopulateTeacherPortalViewBagAsync(userId, cancellationToken);

            var sessions = await mediator.Send(new TeacherAttendanceSessionsRequest
            {
                UserId = userId
            }, cancellationToken);

            var schedule = await mediator.Send(new GetTeacherScheduleRequest
            {
                UserId = userId
            }, cancellationToken);

            var vm = new TeacherAttendanceIndexViewModel
            {
                CurrentWeekType = schedule.CurrentWeekType,
                Days = schedule.Days,
                Sessions = sessions
            };

            return View("TeacherIndex", vm);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet]
        public async Task<IActionResult> Session(int lessonScheduleId, DateTime sessionDate, CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            await PopulateTeacherPortalViewBagAsync(userId, cancellationToken);

            var session = await mediator.Send(new TeacherAttendanceSessionDetailsRequest
            {
                UserId = userId,
                LessonScheduleId = lessonScheduleId,
                SessionDate = sessionDate
            }, cancellationToken);

            var vm = new TeacherAttendanceSessionViewModel
            {
                LessonScheduleId = session.LessonScheduleId,
                SessionDate = session.SessionDate,
                SubjectName = session.SubjectName,
                GroupName = session.GroupName,
                TeacherFullName = session.TeacherFullName,
                RoomDisplay = session.RoomDisplay,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                IsSessionLocked = session.IsSessionLocked,
                SessionMarkedAt = session.SessionMarkedAt,
                SessionLockAt = session.SessionLockAt,
                Students = session.Students
            };

            return View("TeacherSession", vm);
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Session(TeacherMarkAttendanceRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetRequiredUserId();
            await PopulateTeacherPortalViewBagAsync(request.UserId, cancellationToken);

            if (!ModelState.IsValid)
            {
                return await RebuildTeacherSessionViewAsync(request, cancellationToken);
            }

            try
            {
                await mediator.Send(request, cancellationToken);
                return RedirectToAction(nameof(Session), new
                {
                    lessonScheduleId = request.LessonScheduleId,
                    sessionDate = request.SessionDate.ToString("yyyy-MM-dd")
                });
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return await RebuildTeacherSessionViewAsync(request, cancellationToken);
        }

        [Authorize(Roles = "STUDENT")]
        [HttpGet]
        public async Task<IActionResult> Student(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            await PopulateStudentPortalViewBagAsync(userId, cancellationToken);

            var records = await mediator.Send(new StudentAttendanceRequest
            {
                UserId = userId
            }, cancellationToken);

            return View("StudentIndex", records);
        }

        private async Task PopulateTeacherPortalViewBagAsync(int userId, CancellationToken cancellationToken)
        {
            var profile = await mediator.Send(new GetTeacherPortalProfileRequest
            {
                UserId = userId
            }, cancellationToken);

            ViewBag.Title = "Davamiyyət";
            ViewBag.PortalFullName = profile?.FullName ?? string.Empty;
            ViewBag.PortalBadge = profile?.TeacherNumber ?? string.Empty;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile?.FullName ?? string.Empty);
            ViewBag.PortalActiveNav = "attendance";
            ViewBag.PortalIsTeacher = true;
            ViewBag.PortalBreadcrumb = "Davamiyyət";
        }

        private async Task PopulateStudentPortalViewBagAsync(int userId, CancellationToken cancellationToken)
        {
            var profile = await mediator.Send(new GetStudentPortalProfileRequest
            {
                UserId = userId
            }, cancellationToken);

            ViewBag.Title = "Davamiyyət";
            ViewBag.PortalFullName = profile?.FullName ?? string.Empty;
            ViewBag.PortalBadge = profile?.StudentNumber ?? string.Empty;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile?.FullName ?? string.Empty);
            ViewBag.PortalActiveNav = "attendance";
            ViewBag.PortalIsTeacher = false;
            ViewBag.PortalBreadcrumb = "Davamiyyət";
        }

        private async Task<IActionResult> RebuildTeacherSessionViewAsync(TeacherMarkAttendanceRequest request, CancellationToken cancellationToken)
        {
            var session = await mediator.Send(new TeacherAttendanceSessionDetailsRequest
            {
                UserId = request.UserId,
                LessonScheduleId = request.LessonScheduleId,
                SessionDate = request.SessionDate
            }, cancellationToken);

            foreach (var student in session.Students)
            {
                var posted = request.Students?.FirstOrDefault(s => s.StudentId == student.StudentId);
                if (posted is not null)
                    student.Status = posted.Status;
            }

            var vm = new TeacherAttendanceSessionViewModel
            {
                LessonScheduleId = session.LessonScheduleId,
                SessionDate = session.SessionDate,
                SubjectName = session.SubjectName,
                GroupName = session.GroupName,
                TeacherFullName = session.TeacherFullName,
                RoomDisplay = session.RoomDisplay,
                DayOfWeek = session.DayOfWeek,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                IsSessionLocked = session.IsSessionLocked,
                SessionMarkedAt = session.SessionMarkedAt,
                SessionLockAt = session.SessionLockAt,
                Students = session.Students
            };

            return View("TeacherSession", vm);
        }
    }
}
