using Application.Identity;
using Application.Repositories;
using Application.Services;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using Domain.Models.Stables;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.TeachersModule.Commands.TeacherRegisterCommand
{
    public class TeacherRegisterRequestHandler : IRequestHandler<TeacherRegisterRequest, TeacherRegisterResponseDto>
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly IGovernmentIdentityService governmentIdentityService;
        private readonly IDepartmentRepository departmentRepository;
        private readonly UserManager<AppUser> userManager;

        private static string GenerateTeacherNumber()
        {
            var today = DateTime.UtcNow;
            var seq = Random.Shared.Next(100, 999);
            return $"T{today:yyyyMMdd}{seq}";
        }

        public TeacherRegisterRequestHandler(
            ITeacherRepository teacherRepository,
            IGovernmentIdentityService governmentIdentityService,
            IDepartmentRepository departmentRepository,
            UserManager<AppUser> userManager)
        {
            this.teacherRepository = teacherRepository;
            this.governmentIdentityService = governmentIdentityService;
            this.departmentRepository = departmentRepository;
            this.userManager = userManager;
        }

        public async Task<TeacherRegisterResponseDto> Handle(
            TeacherRegisterRequest request,
            CancellationToken cancellationToken)
        {
            var gov = await governmentIdentityService.VerifyTeacherAsync(
                request.SerialNumber, request.FinCode, cancellationToken);

            if (gov is null)
                throw new BadRequestException("FIN kod və ya sənədin seriya nömrəsi yanlışdır.");

            var finNorm = IdentityDocumentSerialNormalizer.Normalize(request.FinCode);
            var serialNorm = IdentityDocumentSerialNormalizer.Normalize(request.SerialNumber);
            if (string.IsNullOrEmpty(serialNorm))
                throw new BadRequestException("Sənədin seriya nömrəsi tələb olunur.");

            var existingByFin = await teacherRepository
                .GetByFinCodeAsync(finNorm, cancellationToken);

            if (existingByFin is not null)
                throw new ConflictException("Bu FIN kod artıq qeydiyyatdan keçmişdir.");

            var existingBySerial = await teacherRepository
                .GetByDocumentSerialNumberAsync(serialNorm, cancellationToken);

            if (existingBySerial is not null)
                throw new ConflictException("Bu sənədin seriya nömrəsi artıq qeydiyyatdan keçmişdir.");

            var departmentName = gov.Department.ToSeededDepartmentName();
            var department = await departmentRepository
                .GetByNameAsync(departmentName, cancellationToken);

            if (department is null)
                throw new BadRequestException($"'{departmentName}' şöbəsi sistemdə mövcud deyil.");

            const string defaultPassword = "Education123!";
            var teacherNumber = GenerateTeacherNumber();

            var user = new AppUser
            {
                UserName = teacherNumber,
                Email = $"{teacherNumber}@lms.edu.az"
            };

            var createResult = await userManager.CreateAsync(user, defaultPassword);
            if (!createResult.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", createResult.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, "TEACHER");

            var teacher = new Teacher
            {
                FullName = gov.FullName,
                BirthDate = gov.BirthDate,
                Experience = gov.Experience,
                FinCode = finNorm,
                DocumentSerialNumber = serialNorm,
                TeacherNumber = teacherNumber,
                MobileNumber = gov.MobileNumber ?? string.Empty,
                Email = user.Email,
                UserId = user.Id,
                TeacherDepartments =
                [
                    new TeacherDepartment { DepartmentId = department.Id }
                ]
            };

            await teacherRepository.AddAsync(teacher, cancellationToken);
            await teacherRepository.SaveAsync(cancellationToken);

            return new TeacherRegisterResponseDto(teacherNumber, defaultPassword);
        }
    }
}
