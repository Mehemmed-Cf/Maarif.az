// Presentation/Areas/Admin/Controllers/AuthController.cs
using Application.Modules.AdminModule.Commands.AdminLoginCommand;
using Application.Modules.StudentsModule.Commands.StudentLoginCommand;
using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
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

        // ── Admin Login ──────────────────────────────────────────────
        
        [HttpGet]
        public IActionResult AdminLogin(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "admin" });
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(AdminLoginRequest request, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(request);

            var result = await mediator.Send(request);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Login failed.");
                return View(request);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Dashboard", new { area = "admin" });
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