using Application.Modules.AttendanceModule.Queries.TeacherAttendanceSessionsQuery;
using Application.Modules.SubjectsModule.Queries.PortalSubjectQuery;
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
    [Authorize(Roles = "TEACHER")]
    public class TeacherPortalController : Controller
    {
        private readonly IMediator mediator;

        public TeacherPortalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetTeacherPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.TeacherLogin), "Auth");

            var schedule = await mediator.Send(
                new GetTeacherScheduleRequest { UserId = userId },
                cancellationToken);

            var vm = new TeacherPortalViewModel
            {
                FullName = profile.FullName,
                TeacherNumber = profile.TeacherNumber,
                CurrentWeekType = schedule.CurrentWeekType,
                Days = schedule.Days
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetTeacherPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.TeacherLogin), "Auth");

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            await mediator.Send(
                new TeacherAttendanceSessionsRequest { UserId = userId },
                cancellationToken);

            return RedirectToAction("Index", "Attendance");
        }

        [HttpGet]
        public async Task<IActionResult> Subjects(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetTeacherPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.TeacherLogin), "Auth");

            PopulateTeacherSubjectViewBag(profile, "Fənlər", "Fənlər");

            var nav = await mediator.Send(
                new GetPortalSubjectNavRequest { UserId = userId, ForTeacher = true },
                cancellationToken);

            if (nav.Count == 0)
                return View("/Views/Shared/PortalNoSubjects.cshtml");

            return RedirectToAction(nameof(Subject), new { id = nav[0].Id });
        }

        [HttpGet]
        public async Task<IActionResult> Subject(int id, CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetTeacherPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.TeacherLogin), "Auth");

            try
            {
                var workspace = await mediator.Send(
                    new GetPortalSubjectWorkspaceRequest
                    {
                        UserId = userId,
                        SubjectId = id,
                        ForTeacher = true
                    },
                    cancellationToken);

                PopulateTeacherSubjectViewBag(profile, workspace.Subject.Name, workspace.Subject.Name);

                var vm = new SubjectWorkspacePageViewModel
                {
                    Workspace = workspace,
                    IsTeacher = true
                };

                return View("/Views/Shared/SubjectWorkspace.cshtml", vm);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        private void PopulateTeacherSubjectViewBag(
            TeacherPortalProfileDto profile,
            string title,
            string breadcrumb)
        {
            ViewBag.Title = title;
            ViewBag.PortalFullName = profile.FullName;
            ViewBag.PortalBadge = profile.TeacherNumber;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile.FullName);
            ViewBag.PortalActiveNav = "subjects";
            ViewBag.PortalIsTeacher = true;
            ViewBag.PortalBreadcrumb = breadcrumb;
        }
    }
}
