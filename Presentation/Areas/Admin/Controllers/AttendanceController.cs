using Application.Modules.AttendanceModule.Commands.AdminEditAttendanceCommand;
using Application.Modules.AttendanceModule.Queries.AdminAttendanceListQuery;
using Application.Modules.AttendanceModule.Queries.AttendanceGetByIdQuery;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.AppCode.Extensions;
using Presentation.AppCode.ViewModels;
using FluentValidation;

namespace Presentation.Areas.Admin.Controllers
{
    public class AttendanceController : AdminBaseController
    {
        private readonly IMediator mediator;
        private readonly IValidator<AdminEditAttendanceRequest> adminEditAttendanceValidator;

        public AttendanceController(
            IMediator mediator,
            IValidator<AdminEditAttendanceRequest> adminEditAttendanceValidator)
        {
            this.mediator = mediator;
            this.adminEditAttendanceValidator = adminEditAttendanceValidator;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new AdminAttendanceListRequest(), cancellationToken);
            return View(response);
        }

        public async Task<IActionResult> Details([FromRoute] AttendanceGetByIdRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new AttendanceGetByIdRequest { Id = id }, cancellationToken);
            return View(new AdminAttendanceEditViewModel
            {
                Attendance = response,
                Form = new AdminEditAttendanceRequest
                {
                    AttendanceId = response.Id,
                    Status = response.Status
                }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminEditAttendanceRequest request, CancellationToken cancellationToken)
        {
            request.UserId = User.GetRequiredUserId();

            var validationResult = await adminEditAttendanceValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                var current = await mediator.Send(new AttendanceGetByIdRequest { Id = request.AttendanceId }, cancellationToken);
                return View(new AdminAttendanceEditViewModel
                {
                    Attendance = current,
                    Form = request
                });
            }

            try
            {
                await mediator.Send(request, cancellationToken);
                return RedirectToAction(nameof(Details), new { id = request.AttendanceId });
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

            var attendance = await mediator.Send(new AttendanceGetByIdRequest { Id = request.AttendanceId }, cancellationToken);
            return View(new AdminAttendanceEditViewModel
            {
                Attendance = attendance,
                Form = request
            });
        }
    }
}
