using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Modules.StudentsModule.Commands.StudentAddCommand
{
    public class StudentAddRequestHandler : IRequestHandler<StudentAddRequest, StudentAddResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        private const string DefaultPassword = "Education123!";

        public StudentAddRequestHandler(
            IStudentRepository studentRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            this.studentRepository = studentRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<StudentAddResponseDto> Handle(
            StudentAddRequest request,
            CancellationToken cancellationToken)
        {
            _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            var fin = request.FinCode.Trim().ToUpperInvariant();
            var studentNumber = request.StudentNumber.Trim();

            if (await studentRepository.GetByFinCodeAsync(fin, cancellationToken) is not null)
                throw new ConflictException("Bu FIN kod artıq istifadə olunur.");

            if (await studentRepository.GetByStudentNumberAsync(studentNumber, cancellationToken) is not null)
                throw new ConflictException("Bu tələbə nömrəsi artıq mövcuddur.");

            if (await userManager.FindByNameAsync(studentNumber) is not null)
                throw new ConflictException("Bu istifadəçi adı (tələbə nömrəsi) artıq qeydiyyatdadır.");

            var user = new AppUser
            {
                UserName = studentNumber,
                Email = $"{studentNumber}@lms.edu.az"
            };

            var createResult = await userManager.CreateAsync(user, DefaultPassword);
            if (!createResult.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", createResult.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(user, "STUDENT");

            var student = mapper.Map<Student>(request);
            student.FinCode = fin;
            student.StudentNumber = studentNumber;
            student.UserId = user.Id;

            await studentRepository.AddAsync(student, cancellationToken);
            await studentRepository.SaveAsync(cancellationToken);

            return mapper.Map<StudentAddResponseDto>(student);
        }
    }
}