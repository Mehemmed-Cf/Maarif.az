using Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery;
using Application.Modules.TeachersModule.Queries.GetTeacherScheduleQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
