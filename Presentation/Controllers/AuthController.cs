// Presentation/Areas/Admin/Controllers/AuthController.cs
using Application.Modules.AdminModule.Commands.AdminLoginCommand;
using Application.Modules.StudentsModule.Commands.ForgotStudentPasswordCommand;
using Application.Modules.StudentsModule.Commands.RecoverStudentNumberCommand;
using Application.Modules.StudentsModule.Commands.StudentChangePasswordCommand;
using Application.Modules.StudentsModule.Commands.StudentLoginCommand;
using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Application.Modules.StudentsModule.Queries.GetStudentPortalProfileQuery;
using Application.Modules.TeachersModule.Commands.ForgotTeacherPasswordCommand;
using Application.Modules.TeachersModule.Commands.RecoverTeacherNumberCommand;
using Application.Modules.TeachersModule.Commands.TeacherChangePasswordCommand;
using Application.Modules.TeachersModule.Commands.TeacherLoginCommand;
using Application.Modules.TeachersModule.Commands.TeacherRegisterCommand;
using Application.Modules.TeachersModule.Queries.GetTeacherPortalProfileQuery;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.AppCode;
using Presentation.AppCode.Extensions;

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

        // ── Teacher register ──────────────────────────────────────────

        [HttpGet]
        public IActionResult TeacherRegister() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherRegister(TeacherRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request);
                TempData["TeacherNumber"] = result.TeacherNumber;
                TempData["TeacherDefaultPassword"] = result.DefaultPassword;
                return RedirectToAction(nameof(TeacherRegisterSuccess));
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
        public IActionResult TeacherRegisterSuccess()
        {
            if (TempData["TeacherNumber"] is null)
                return RedirectToAction(nameof(TeacherRegister));

            ViewBag.TeacherNumber = TempData["TeacherNumber"];
            ViewBag.DefaultPassword = TempData["TeacherDefaultPassword"];
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
                HttpContext.Session.SetString("StudentFullName", result.FullName);
                HttpContext.Session.SetString("StudentNumber", result.StudentNumber);
                return RedirectToAction("Index", "Portal");
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

        // ── Teacher login ─────────────────────────────────────────────

        [HttpGet]
        public IActionResult TeacherLogin() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherLogin(TeacherLoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request);
                HttpContext.Session.SetString("TeacherFullName", result.FullName);
                HttpContext.Session.SetString("TeacherNumber", result.TeacherNumber);
                return RedirectToAction("Index", "TeacherPortal");
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

        // ── Access denied (wrong role / admin area) ───────────────────

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // ── Admin Login ──────────────────────────────────────────────
        
        [HttpGet]
        public IActionResult AdminLogin(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("SUPERADMIN"))
                    return RedirectToAction("Index", "Dashboard", new { area = "admin" });
                if (User.IsInRole("STUDENT"))
                    return RedirectToAction("Index", "Portal");
                if (User.IsInRole("TEACHER"))
                    return RedirectToAction("Index", "TeacherPortal");
                return RedirectToAction(nameof(AccessDenied));
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

        // ── Change password (signed-in) ───────────────────────────────

        [Authorize(Roles = "STUDENT")]
        [HttpGet]
        public async Task<IActionResult> StudentChangePassword(CancellationToken cancellationToken)
        {
            if (!await TryFillStudentPortalChromeAsync(cancellationToken))
                return RedirectToAction(nameof(Login));
            ViewBag.PortalActiveNav = "password";
            ViewBag.PortalBreadcrumb = "Şifrəni dəyiş";
            return View(new StudentChangePasswordRequest());
        }

        [Authorize(Roles = "STUDENT")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentChangePassword(
            [Bind(nameof(StudentChangePasswordRequest.CurrentPassword), nameof(StudentChangePasswordRequest.NewPassword), nameof(StudentChangePasswordRequest.ConfirmNewPassword))]
            StudentChangePasswordRequest request,
            CancellationToken cancellationToken)
        {
            if (!await TryFillStudentPortalChromeAsync(cancellationToken))
                return RedirectToAction(nameof(Login));
            ViewBag.PortalActiveNav = "password";
            ViewBag.PortalBreadcrumb = "Şifrəni dəyiş";

            if (!ModelState.IsValid)
                return View(request);

            try
            {
                request.UserId = User.GetRequiredUserId();
                await mediator.Send(request, cancellationToken);
                TempData["PasswordMessage"] = "Şifrə uğurla yeniləndi.";
                return RedirectToAction(nameof(StudentChangePassword));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }

        [Authorize(Roles = "TEACHER")]
        [HttpGet]
        public async Task<IActionResult> TeacherChangePassword(CancellationToken cancellationToken)
        {
            if (!await TryFillTeacherPortalChromeAsync(cancellationToken))
                return RedirectToAction(nameof(TeacherLogin));
            ViewBag.PortalActiveNav = "password";
            ViewBag.PortalBreadcrumb = "Şifrəni dəyiş";
            return View(new TeacherChangePasswordRequest());
        }

        [Authorize(Roles = "TEACHER")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherChangePassword(
            [Bind(nameof(TeacherChangePasswordRequest.CurrentPassword), nameof(TeacherChangePasswordRequest.NewPassword), nameof(TeacherChangePasswordRequest.ConfirmNewPassword))]
            TeacherChangePasswordRequest request,
            CancellationToken cancellationToken)
        {
            if (!await TryFillTeacherPortalChromeAsync(cancellationToken))
                return RedirectToAction(nameof(TeacherLogin));
            ViewBag.PortalActiveNav = "password";
            ViewBag.PortalBreadcrumb = "Şifrəni dəyiş";

            if (!ModelState.IsValid)
                return View(request);

            try
            {
                request.UserId = User.GetRequiredUserId();
                await mediator.Send(request, cancellationToken);
                TempData["PasswordMessage"] = "Şifrə uğurla yeniləndi.";
                return RedirectToAction(nameof(TeacherChangePassword));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
            }
        }

        // ── Forgot password (identity check) ──────────────────────────

        [HttpGet]
        public IActionResult ForgotStudentPassword() => View(new ForgotStudentPasswordRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotStudentPassword(ForgotStudentPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                await mediator.Send(request, HttpContext.RequestAborted);
                TempData["AuthInfo"] = "Yeni şifrə ilə daxil ola bilərsiniz.";
                return RedirectToAction(nameof(Login));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
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

        [HttpGet]
        public IActionResult ForgotTeacherPassword() => View(new ForgotTeacherPasswordRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotTeacherPassword(ForgotTeacherPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                await mediator.Send(request, HttpContext.RequestAborted);
                TempData["AuthInfo"] = "Yeni şifrə ilə daxil ola bilərsiniz.";
                return RedirectToAction(nameof(TeacherLogin));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
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

        // ── Recover login number ──────────────────────────────────────

        [HttpGet]
        public IActionResult RecoverStudentNumber() => View(new RecoverStudentNumberRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverStudentNumber(RecoverStudentNumberRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request, HttpContext.RequestAborted);
                TempData["RecoveredStudentNumber"] = result.StudentNumber;
                return RedirectToAction(nameof(RecoverStudentNumberSuccess));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
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

        [HttpGet]
        public IActionResult RecoverStudentNumberSuccess()
        {
            if (TempData["RecoveredStudentNumber"] is null)
                return RedirectToAction(nameof(RecoverStudentNumber));
            ViewBag.RecoveredNumber = TempData["RecoveredStudentNumber"];
            return View();
        }

        [HttpGet]
        public IActionResult RecoverTeacherNumber() => View(new RecoverTeacherNumberRequest());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverTeacherNumber(RecoverTeacherNumberRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var result = await mediator.Send(request, HttpContext.RequestAborted);
                TempData["RecoveredTeacherNumber"] = result.TeacherNumber;
                return RedirectToAction(nameof(RecoverTeacherNumberSuccess));
            }
            catch (BadRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(request);
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

        [HttpGet]
        public IActionResult RecoverTeacherNumberSuccess()
        {
            if (TempData["RecoveredTeacherNumber"] is null)
                return RedirectToAction(nameof(RecoverTeacherNumber));
            ViewBag.RecoveredNumber = TempData["RecoveredTeacherNumber"];
            return View();
        }

        // ── Logout ───────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string? returnPortal = null)
        {
            await HttpContext.SignOutAsync();
            if (string.Equals(returnPortal, "teacher", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(TeacherLogin));
            return RedirectToAction(nameof(Login));
        }

        private async Task<bool> TryFillStudentPortalChromeAsync(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(new GetStudentPortalProfileRequest { UserId = userId }, cancellationToken);
            if (profile is null)
                return false;
            ViewBag.Title = "Şifrəni dəyiş";
            ViewBag.PortalFullName = profile.FullName;
            ViewBag.PortalBadge = profile.StudentNumber;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile.FullName);
            ViewBag.PortalIsTeacher = false;
            return true;
        }

        private async Task<bool> TryFillTeacherPortalChromeAsync(CancellationToken cancellationToken)
        {
            var userId = User.GetRequiredUserId();
            var profile = await mediator.Send(new GetTeacherPortalProfileRequest { UserId = userId }, cancellationToken);
            if (profile is null)
                return false;
            ViewBag.Title = "Şifrəni dəyiş";
            ViewBag.PortalFullName = profile.FullName;
            ViewBag.PortalBadge = profile.TeacherNumber;
            ViewBag.PortalInitials = PortalText.InitialsFrom(profile.FullName);
            ViewBag.PortalIsTeacher = true;
            return true;
        }
    }
}