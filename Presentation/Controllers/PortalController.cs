using Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery;
using Application.Modules.StudentsModule.Queries.GetStudentScheduleQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    }
}
