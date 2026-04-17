using Application.Modules.SubjectsModule;
using Application.Repositories;
using AutoMapper;
using Domain.Models.Entities;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.SubjectsModule.Commands.SubjectEditCommand
{
    public class SubjectEditRequestHandler : IRequestHandler<SubjectEditRequest, SubjectEditResponseDto>
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ISubjectTopicRepository subjectTopicRepository;
        private readonly ISubjectMaterialRepository subjectMaterialRepository;
        private readonly ISubjectLiteratureRepository subjectLiteratureRepository;
        private readonly IMapper mapper;

        public SubjectEditRequestHandler(
            ISubjectRepository subjectRepository,
            IDepartmentRepository departmentRepository,
            ISubjectTopicRepository subjectTopicRepository,
            ISubjectMaterialRepository subjectMaterialRepository,
            ISubjectLiteratureRepository subjectLiteratureRepository,
            IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.departmentRepository = departmentRepository;
            this.subjectTopicRepository = subjectTopicRepository;
            this.subjectMaterialRepository = subjectMaterialRepository;
            this.subjectLiteratureRepository = subjectLiteratureRepository;
            this.mapper = mapper;
        }

        public async Task<SubjectEditResponseDto> Handle(
            SubjectEditRequest request,
            CancellationToken cancellationToken)
        {
            var entity = await subjectRepository.GetByIdWithDetailsTrackedAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            if (entity.DepartmentId != request.DepartmentId)
                _ = await departmentRepository.GetAsync(d => d.Id == request.DepartmentId, cancellationToken);

            entity.Name = request.Name.Trim();
            entity.DepartmentId = request.DepartmentId;
            entity.Term = request.Term.Trim();
            entity.GroupName = request.GroupName.Trim();
            entity.LectureTeacher = request.LectureTeacher;
            entity.SeminarTeacher = request.SeminarTeacher;
            entity.LabTeacher = request.LabTeacher;
            entity.StudentCount = request.StudentCount;
            entity.Credits = request.Credits;
            entity.TotalHours = request.TotalHours;
            entity.WeekCount = request.WeekCount;
            entity.Purpose = request.Purpose;
            entity.TeacherMethods = request.TeacherMethods;
            entity.SyllabusUrl = request.SyllabusUrl;
            entity.FreeWorkScore = request.FreeWorkScore;
            entity.SeminarScore = request.SeminarScore;
            entity.LabScore = request.LabScore;
            entity.AttendanceScore = request.AttendanceScore;
            entity.ExamScore = request.ExamScore;

            entity.Topics ??= new List<SubjectTopic>();
            entity.Materials ??= new List<SubjectMaterial>();
            entity.Literatures ??= new List<SubjectLiterature>();

            SyncTopics(entity, request.Topics);
            SyncMaterials(entity, request.Materials);
            SyncLiteratures(entity, request.Literatures);

            await subjectRepository.EditAsync(entity);
            await subjectRepository.SaveAsync(cancellationToken);

            var updated = await subjectRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken)
                ?? throw new NotFoundException($"Fənn tapılmadı (Id: {request.Id})");

            return mapper.Map<SubjectEditResponseDto>(updated);
        }

        private void SyncTopics(Subject entity, List<SubjectTopicRowDto> rows)
        {
            var incoming = (rows ?? new List<SubjectTopicRowDto>())
                .Where(r => !string.IsNullOrWhiteSpace(r.TopicName))
                .ToList();

            var keepIds = incoming.Where(r => r.Id > 0).Select(r => r.Id).ToHashSet();

            foreach (var existing in entity.Topics.ToList())
            {
                if (!keepIds.Contains(existing.Id))
                    subjectTopicRepository.Remove(existing);
            }

            foreach (var row in incoming.Where(r => r.Id > 0))
            {
                var topic = entity.Topics.FirstOrDefault(t => t.Id == row.Id);
                if (topic is null || topic.SubjectId != entity.Id)
                    continue;

                topic.WeekNumber = row.WeekNumber;
                topic.TopicName = row.TopicName.Trim();
                topic.TeachingMethods = row.TeachingMethods;
                topic.Materials = row.Materials;
                topic.Equipment = row.Equipment;
            }

            foreach (var row in incoming.Where(r => r.Id <= 0))
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
        }

        private void SyncMaterials(Subject entity, List<SubjectMaterialRowDto> rows)
        {
            var incoming = (rows ?? new List<SubjectMaterialRowDto>())
                .Where(r => !string.IsNullOrWhiteSpace(r.Title) && !string.IsNullOrWhiteSpace(r.FileUrl))
                .ToList();

            var keepIds = incoming.Where(r => r.Id > 0).Select(r => r.Id).ToHashSet();

            foreach (var existing in entity.Materials.ToList())
            {
                if (!keepIds.Contains(existing.Id))
                    subjectMaterialRepository.Remove(existing);
            }

            foreach (var row in incoming.Where(r => r.Id > 0))
            {
                var m = entity.Materials.FirstOrDefault(t => t.Id == row.Id);
                if (m is null || m.SubjectId != entity.Id)
                    continue;

                m.Title = row.Title.Trim();
                m.Description = row.Description;
                m.FileUrl = row.FileUrl.Trim();
                m.MaterialType = string.IsNullOrWhiteSpace(row.MaterialType) ? "General" : row.MaterialType.Trim();
            }

            foreach (var row in incoming.Where(r => r.Id <= 0))
            {
                entity.Materials.Add(new SubjectMaterial
                {
                    Title = row.Title.Trim(),
                    Description = row.Description,
                    FileUrl = row.FileUrl.Trim(),
                    MaterialType = string.IsNullOrWhiteSpace(row.MaterialType) ? "General" : row.MaterialType.Trim()
                });
            }
        }

        private void SyncLiteratures(Subject entity, List<SubjectLiteratureRowDto> rows)
        {
            var incoming = (rows ?? new List<SubjectLiteratureRowDto>())
                .Where(r => !string.IsNullOrWhiteSpace(r.BookName) && !string.IsNullOrWhiteSpace(r.Author))
                .ToList();

            var keepIds = incoming.Where(r => r.Id > 0).Select(r => r.Id).ToHashSet();

            foreach (var existing in entity.Literatures.ToList())
            {
                if (!keepIds.Contains(existing.Id))
                    subjectLiteratureRepository.Remove(existing);
            }

            foreach (var row in incoming.Where(r => r.Id > 0))
            {
                var lit = entity.Literatures.FirstOrDefault(t => t.Id == row.Id);
                if (lit is null || lit.SubjectId != entity.Id)
                    continue;

                lit.Type = string.IsNullOrWhiteSpace(row.Type) ? "Əsas" : row.Type.Trim();
                lit.Author = row.Author.Trim();
                lit.BookName = row.BookName.Trim();
                lit.Publisher = row.Publisher;
                lit.PublicationYear = row.PublicationYear;
            }

            foreach (var row in incoming.Where(r => r.Id <= 0))
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
        }
    }
}
