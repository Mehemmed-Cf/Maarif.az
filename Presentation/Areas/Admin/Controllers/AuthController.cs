// Presentation/Areas/Admin/Controllers/AuthController.cs
using Application.Modules.StudentsModule.Commands.StudentLoginCommand;
using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("admin")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // ── Register ────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(StudentRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request);

                TempData["StudentNumber"] = result.StudentNumber;
                TempData["DefaultPassword"] = result.DefaultPassword;

                return RedirectToAction(nameof(RegisterSuccess));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            if (TempData["StudentNumber"] is null)
                return RedirectToAction(nameof(Register));

            ViewBag.StudentNumber = TempData["StudentNumber"];
            ViewBag.DefaultPassword = TempData["DefaultPassword"];

            return View();
        }

        // ── Login ────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(StudentLoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request);
                return RedirectToAction("Index", "Dashboard", new { area = "admin" });
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
            catch (UnauthorizedException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }

        // ── Logout ───────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}