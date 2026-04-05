using Application.Repositories;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.TeachersModule.Commands.TeacherLoginCommand
{
    public class TeacherLoginRequestHandler
        : IRequestHandler<TeacherLoginRequest, TeacherLoginResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public TeacherLoginRequestHandler(
            ITeacherRepository teacherRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.teacherRepository = teacherRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<TeacherLoginResponseDto> Handle(
            TeacherLoginRequest request,
            CancellationToken cancellationToken)
        {
            var teacher = await teacherRepository
                .GetByTeacherNumberAsync(request.TeacherNumber.Trim(), cancellationToken);

            if (teacher is null)
                throw new NotFoundException("Bu müəllim nömrəsi ilə qeydiyyat tapılmadı.");

            var user = await userManager.FindByIdAsync(teacher.UserId.ToString());

            if (user is null)
                throw new NotFoundException("İstifadəçi tapılmadı.");

            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
                throw new UnauthorizedException("Şifrə yanlışdır.");

            await signInManager.SignInAsync(user, isPersistent: false);

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "TEACHER";

            return new TeacherLoginResponseDto(
                teacher.TeacherNumber ?? request.TeacherNumber.Trim(),
                teacher.FullName,
                role);
        }
    }
}
