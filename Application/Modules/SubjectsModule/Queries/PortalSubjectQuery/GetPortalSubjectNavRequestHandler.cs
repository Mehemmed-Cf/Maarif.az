using Application.Repositories;
using MediatR;

namespace Application.Modules.SubjectsModule.Queries.PortalSubjectQuery
{
    public class GetPortalSubjectNavRequestHandler
        : IRequestHandler<GetPortalSubjectNavRequest, IReadOnlyList<PortalSubjectNavItemDto>>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ILessonScheduleRepository scheduleRepository;

        public GetPortalSubjectNavRequestHandler(
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ILessonScheduleRepository scheduleRepository)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
            this.scheduleRepository = scheduleRepository;
        }

        public async Task<IReadOnlyList<PortalSubjectNavItemDto>> Handle(
            GetPortalSubjectNavRequest request,
            CancellationToken cancellationToken)
        {
            if (request.ForTeacher)
                return await BuildTeacherNavAsync(request.UserId, cancellationToken);

            return await BuildStudentNavAsync(request.UserId, cancellationToken);
        }

        private async Task<IReadOnlyList<PortalSubjectNavItemDto>> BuildStudentNavAsync(int userId, CancellationToken ct)
        {
            var student = await studentRepository.GetByUserIdWithDetailsAsync(userId, ct);
            if (student?.StudentGroups is null || student.StudentGroups.Count == 0)
                return Array.Empty<PortalSubjectNavItemDto>();

            var map = new Dictionary<int, string>();
            foreach (var sg in student.StudentGroups)
            {
                var rows = await scheduleRepository.GetByGroupAsync(sg.GroupId, filter: null, ct);
                foreach (var row in rows)
                {
                    var lesson = row.Lesson;
                    if (lesson?.Subject is null)
                        continue;
                    map[lesson.SubjectId] = lesson.Subject.Name;
                }
            }

            return map
                .Select(kv => new PortalSubjectNavItemDto { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private async Task<IReadOnlyList<PortalSubjectNavItemDto>> BuildTeacherNavAsync(int userId, CancellationToken ct)
        {
            var teacher = await teacherRepository.GetByUserIdWithDetailsAsync(userId, ct);
            if (teacher?.Lessons is null || teacher.Lessons.Count == 0)
                return Array.Empty<PortalSubjectNavItemDto>();

            var map = new Dictionary<int, string>();
            foreach (var lesson in teacher.Lessons)
            {
                if (lesson.Subject is null)
                    continue;
                map[lesson.SubjectId] = lesson.Subject.Name;
            }

            return map
                .Select(kv => new PortalSubjectNavItemDto { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
    }
}
