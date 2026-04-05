using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.TeachersModule.Commands.TeacherAddCommand
{
    public class TeacherAddRequestHandler : IRequestHandler<TeacherAddRequest, TeacherResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        private const string DefaultPassword = "Education123!";

        private static string GenerateTeacherNumber()
        {
            var today = DateTime.UtcNow;
            var seq = Random.Shared.Next(100, 999);
            return $"T{today:yyyyMMdd}{seq}";
        }

        public TeacherAddRequestHandler(
            ITeacherRepository teacherRepository,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            this.teacherRepository = teacherRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<TeacherResponseDto> Handle(TeacherAddRequest request, CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();

            var emailTaken = teacherRepository.GetAll()
                .Any(t => t.Email != null
                    && t.Email.Trim().Equals(email, StringComparison.OrdinalIgnoreCase));
            if (emailTaken)
                throw new ConflictException("Bu e-poçt ünvanı artıq başqa müəllimə təyin olunub.");

            if (await userManager.FindByEmailAsync(email) is not null)
                throw new ConflictException("Bu e-poçt ünvanı artıq istifadəçi hesabında mövcuddur.");

            var teacherNumber = GenerateTeacherNumber();
            while (await userManager.FindByNameAsync(teacherNumber) is not null
                   || teacherRepository.GetAll().Any(t => t.TeacherNumber == teacherNumber))
                teacherNumber = GenerateTeacherNumber();

            var user = new AppUser
            {
                UserName = teacherNumber,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, DefaultPassword);
            if (!createResult.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", createResult.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, "TEACHER");

            var teacher = mapper.Map<Teacher>(request);
            teacher.Email = email;
            teacher.TeacherNumber = teacherNumber;
            teacher.UserId = user.Id;
            teacher.TeacherDepartments = request.DepartmentIds.Select(id => new TeacherDepartment
            {
                DepartmentId = id
            }).ToList();

            await teacherRepository.AddAsync(teacher, cancellationToken);
            await teacherRepository.SaveAsync(cancellationToken);

            return mapper.Map<TeacherResponseDto>(teacher);
        }
    }
}
