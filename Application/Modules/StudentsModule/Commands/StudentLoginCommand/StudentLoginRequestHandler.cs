using Application.Repositories;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.StudentsModule.Commands.StudentLoginCommand
{
    public class StudentLoginRequestHandler
        : IRequestHandler<StudentLoginRequest, StudentLoginResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public StudentLoginRequestHandler(
            IStudentRepository studentRepository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            this.studentRepository = studentRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<StudentLoginResponseDto> Handle(
            StudentLoginRequest request,
            CancellationToken cancellationToken)
        {
            // 1. Find student by student number
            var student = await studentRepository
                .GetByStudentNumberAsync(request.StudentNumber, cancellationToken);

            if (student is null)
                throw new NotFoundException(
                    "Bu tələbə nömrəsi ilə qeydiyyat tapılmadı.");


            // 2. Find the identity user
            var user = await userManager.FindByIdAsync(student.UserId.ToString());

            if (user is null)
                throw new NotFoundException("İstifadəçi tapılmadı.");

            // 3. Check password
            var passwordValid = await userManager
                .CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
                throw new UnauthorizedException("Şifrə yanlışdır.");

            // 4. Sign in — this sets the auth cookie
            await signInManager.SignInAsync(user, isPersistent: false);

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "STUDENT";

            return new StudentLoginResponseDto(
                student.StudentNumber,
                student.FullName,
                role);
        }
    }
}