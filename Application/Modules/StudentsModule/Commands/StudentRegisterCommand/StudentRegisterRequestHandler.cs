using Application.Repositories;
using Application.Services;
using Domain.Models.Entities;
using Domain.Models.Entities.Membership;
using Domain.Models.Stables;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Polly;
using Polly.Retry;

namespace Application.Modules.StudentsModule.Commands.StudentRegisterCommand
{
    public class StudentRegisterRequestHandler : IRequestHandler<StudentRegisterRequest, StudentRegisterResponseDto>
    {
        private readonly IStudentRepository studentRepository;
        private readonly IGovernmentIdentityService governmentIdentityService;
        private readonly IDepartmentRepository departmentRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly AsyncRetryPolicy<dynamic> _apiRetryPolicy;

        private static string GenerateStudentNumber()
        {
            // Format: YYYYMMDD + 3-digit random — replace with a proper sequence in production
            var today = DateTime.UtcNow;
            var seq = Random.Shared.Next(100, 999);
            return $"{today:yyyyMMdd}{seq}";
        }

        public StudentRegisterRequestHandler(
            IStudentRepository studentRepository, 
            IGovernmentIdentityService governmentIdentityService, 
            IDepartmentRepository departmentRepository, 
            UserManager<AppUser> userManager)
        {
            this.studentRepository = studentRepository;
            this.governmentIdentityService = governmentIdentityService;
            this.departmentRepository = departmentRepository;
            this.userManager = userManager;

            // Recommendation C: Configure Polly Retry Policy
            // Retries 3 times with an exponential backoff if a transient error occurs
            _apiRetryPolicy = Policy<dynamic>
                .Handle<Exception>() // In production, narrow this down to HttpRequestException or TimeoutException
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        public async Task<StudentRegisterResponseDto> Handle(StudentRegisterRequest request, CancellationToken cancellationToken)
        {
            // 1. Verify identity against government API
            //var finData = await governmentIdentityService.VerifyAsync(
            //    request.SerialNumber, request.FinCode, cancellationToken);

            var finData = await _apiRetryPolicy.ExecuteAsync(async () =>
                await governmentIdentityService.VerifyAsync(request.SerialNumber, request.FinCode, cancellationToken)
            );

            if (finData is null)
                throw new BadRequestException(
                    "FIN kod və ya sənədin seriya nömrəsi yanlışdır.");

            using (IDbContextTransaction transaction = await studentRepository.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    // 2. Guard: already registered
                    var existing = await studentRepository
                        .GetByFinCodeAsync(request.FinCode.Trim().ToUpper(), cancellationToken);

                    if (existing is not null)
                        throw new ConflictException(
                            "Bu FIN kod artıq qeydiyyatdan keçmişdir.");

                    var departmentName = finData.Department.ToString();
                    var department = await departmentRepository
                        .GetByNameAsync(departmentName, cancellationToken);

                    if (department is null)
                        throw new BadRequestException(
                            $"'{departmentName}' şöbəsi sistemdə mövcud deyil.");

                    // 3. Create Identity user
                    const string defaultPassword = "Education123!";
                    var studentNumber = GenerateStudentNumber();

                    var user = new AppUser
                    {
                        UserName = studentNumber,
                        Email = $"{studentNumber}@lms.edu.az"
                    };

                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (!result.Succeeded)
                        throw new BadRequestException(
                            string.Join(", ", result.Errors.Select(e => e.Description)));

                    await userManager.AddToRoleAsync(user, "STUDENT");

                    // 4. Create student entity (audit fields set by DataContext.SaveChangesAsync)
                    var student = new Student
                    {
                        FullName = finData.FullName,
                        FatherName = finData.FatherName,
                        BirthDate = finData.BirthDate,
                        Gender = finData.Gender,
                        FinCode = finData.FinCode,
                        StudentNumber = studentNumber,
                        Status = StatusType.Active,
                        MobileNumber = string.Empty,
                        UserId = user.Id,
                        DepartmentId = department.Id,
                        EducationType = finData.Education,
                        Year = 1,
                        Grade = GradeType.F
                    };

                    await studentRepository.AddAsync(student, cancellationToken);
                    await studentRepository.SaveAsync(cancellationToken);

                    return new StudentRegisterResponseDto(studentNumber, defaultPassword);
                } catch (Exception)
                {
                    // If any error occurs (DB timeout, constraint violation, etc.), roll back
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
