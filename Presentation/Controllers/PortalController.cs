using Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery;
using Application.Modules.StudentsModule.Queries.GetStudentScheduleQuery;
using Application.Modules.SubjectsModule.Queries.PortalSubjectQuery;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.AppCode;
using Presentation.AppCode.Extensions;
using Presentation.AppCode.ViewModels;

namespace Presentation.Controllers
{
    [Authorize(Roles = "STUDENT")]
    public class PortalController : Controller
    {
        private readonly IMediator mediator;

        public PortalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetStudentPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.Login), "Auth");

            var schedule = await mediator.Send(
                new GetStudentScheduleRequest { UserId = userId },
                cancellationToken);

            var vm = new PortalViewModel
            {
                FullName = profile.FullName,
                StudentNumber = profile.StudentNumber,
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
                new GetStudentPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.Login), "Auth");

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> Subjects(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(
                new GetStudentPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.Login), "Auth");

            PopulateStudentSubjectViewBag(profile, "Fənlər", "Fənlər");

            var nav = await mediator.Send(
                new GetPortalSubjectNavRequest { UserId = userId, ForTeacher = false },
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
                new GetStudentPortalProfileRequest { UserId = userId },
                cancellationToken);

            if (profile is null)
                return RedirectToAction(nameof(AuthController.Login), "Auth");

            try
            {
                var workspace = await mediator.Send(
                    new GetPortalSubjectWorkspaceRequest
                    {
                        UserId = userId,
                        SubjectId = id,
                        ForTeacher = false
                    },
                    cancellationToken);

                PopulateStudentSubjectViewBag(profile, workspace.Subject.Name, workspace.Subject.Name);

                var vm = new SubjectWorkspacePageViewModel
                {
                    Workspace = workspace,
                    IsTeacher = false
                };

                return View("/Views/Shared/SubjectWorkspace.cshtml", vm);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        private void PopulateStudentSubjectViewBag(
            StudentPortalProfileDto profile,
            string title,
            string breadcrumb)
        {
            ViewBag.Title = title;
            ViewBag.PortalFullName = profile.FullName;
            ViewBag.PortalBadge = profile.StudentNumber;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile.FullName);
            ViewBag.PortalActiveNav = "subjects";
            ViewBag.PortalIsTeacher = false;
            ViewBag.PortalBreadcrumb = breadcrumb;
        }
    }
}
