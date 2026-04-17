using Application.Modules.SubjectsModule;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectAddCommand
{
    public class SubjectAddRequestHandler : IRequestHandler<SubjectAddRequest, SubjectAddResponseDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public SubjectAddRequestHandler(
            ISubjectRepository subjectRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectAddResponseDto> Handle(
            SubjectAddRequest request,
            CancellationToken cancellationToken)
        {
            _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            var term = string.IsNullOrWhiteSpace(request.Term) ? "Seed-Term" : request.Term.Trim();
            var groupName = string.IsNullOrWhiteSpace(request.GroupName)
                ? $"DEPT-{request.DepartmentId}"
                : request.GroupName.Trim();

            var entity = new Subject
            {
                Name = request.Name.Trim(),
                DepartmentId = request.DepartmentId,
                Term = term,
                GroupName = groupName,
                LectureTeacher = request.LectureTeacher,
                SeminarTeacher = request.SeminarTeacher,
                LabTeacher = request.LabTeacher,
                StudentCount = request.StudentCount,
                Credits = request.Credits,
                TotalHours = request.TotalHours,
                WeekCount = request.WeekCount,
                Purpose = request.Purpose,
                TeacherMethods = request.TeacherMethods,
                SyllabusUrl = request.SyllabusUrl,
                FreeWorkScore = request.FreeWorkScore,
                SeminarScore = request.SeminarScore,
                LabScore = request.LabScore,
                AttendanceScore = request.AttendanceScore,
                ExamScore = request.ExamScore,
                Topics = new List<SubjectTopic>(),
                Materials = new List<SubjectMaterial>(),
                Literatures = new List<SubjectLiterature>()
            };

            foreach (var row in request.Topics.Where(r => !string.IsNullOrWhiteSpace(r.TopicName)))
            {
                entity.Topics.Add(new SubjectTopic
                {
                    WeekNumber = row.WeekNumber,
                    TopicName = row.TopicName.Trim(),
                    TeachingMethods = row.TeachingMethods,
                    Materials = row.Materials,
                    Equipment = row.Equipment
                });
            }

            foreach (var row in request.Materials.Where(r =>
                         !string.IsNullOrWhiteSpace(r.Title) && !string.IsNullOrWhiteSpace(r.FileUrl)))
            {
                entity.Materials.Add(new SubjectMaterial
                {
                    Title = row.Title.Trim(),
                    Description = row.Description,
                    FileUrl = row.FileUrl.Trim(),
                    MaterialType = string.IsNullOrWhiteSpace(row.MaterialType) ? "General" : row.MaterialType.Trim()
                });
            }

            foreach (var row in request.Literatures.Where(r =>
                         !string.IsNullOrWhiteSpace(r.BookName) && !string.IsNullOrWhiteSpace(r.Author)))
            {
                entity.Literatures.Add(new SubjectLiterature
                {
                    Type = string.IsNullOrWhiteSpace(row.Type) ? "Əsas" : row.Type.Trim(),
                    Author = row.Author.Trim(),
                    BookName = row.BookName.Trim(),
                    Publisher = row.Publisher,
                    PublicationYear = row.PublicationYear
                });
            }

            await subjectRepository.AddAsync(entity, cancellationToken);
            await subjectRepository.SaveAsync(cancellationToken);

            var created = await subjectRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken)
                ?? throw new InvalidOperationException("Subject was saved but could not be reloaded.");

            return mapper.Map<SubjectAddResponseDto>(created);
        }
    }
}
