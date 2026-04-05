using Application.Repositories;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.StudentsModule.Commands.StudentChangePasswordCommand
{
    public class StudentChangePasswordRequestHandler : IRequestHandler<StudentChangePasswordRequest, Unit>
    {
        private readonly IStudentRepository studentRepository;
        private readonly UserManager<AppUser> userManager;

        public StudentChangePasswordRequestHandler(
            IStudentRepository studentRepository,
            UserManager<AppUser> userManager)
        {
            this.studentRepository = studentRepository;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(StudentChangePasswordRequest request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                throw new BadRequestException("Yeni şifrə və təsdiqi eyni deyil.");

            var student = await studentRepository.GetByUserIdWithDetailsAsync(request.UserId, cancellationToken);
            if (student is null)
                throw new NotFoundException("Tələbə hesabı tapılmadı.");

            var user = await userManager.FindByIdAsync(student.UserId.ToString());
            if (user is null)
                throw new NotFoundException("İstifadəçi tapılmadı.");

            var result = await userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));

            return Unit.Value;
        }
    }
}
