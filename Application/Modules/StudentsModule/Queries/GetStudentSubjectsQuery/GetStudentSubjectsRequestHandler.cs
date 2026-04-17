using Application.Repositories;
using Infrastructure.Exceptions;
using MediatR;

namespace Application.Modules.StudentsModule.Queries.GetStudentSubjectsQuery
{
    //public class GetStudentSubjectsRequestHandler : IRequestHandler<GetStudentSubjectsRequest, IEnumerable<GetStudentSubjectsRequestDto>>
    //{
    //    private readonly ISubjectRepository subjectRepository;
    //    private readonly IStudentRepository studentRepository;

    //    public GetStudentSubjectsRequestHandler(ISubjectRepository subjectRepository, IStudentRepository studentRepository)
    //    {
    //        this.subjectRepository = subjectRepository;
    //        this.studentRepository = studentRepository;
    //    }

    //    public async Task<IEnumerable<GetStudentSubjectsRequestDto>> Handle(GetStudentSubjectsRequest request, CancellationToken cancellationToken)
    //    {
    //        var student = await studentRepository.GetByUserIdWithDetailsAsync(
    //            request.UserId,
    //            cancellationToken);

    //        if (student?.StudentGroups is null || student.StudentGroups.Count == 0)
    //            throw new UserNotFoundException();

    //        var groupIds = student.StudentGroups
    //        .Select(sg => sg.GroupId)
    //        .Distinct()
    //        .ToList();

    //        var subjects = new List<StudentSubjectDto>();

    //        foreach (var groupId in groupIds)
    //        {
    //            //var schedules = await scheduleRepository.GetByGroupAsync(
    //            //    groupId,
    //            //    filter: null,
    //            //    cancellationToken);

    //            var groupSubjects = await subjectRepository.GetByGroupIdAsync(groupId, cancellationToken);
    //            subjects.AddRange(groupSubjects.Select(gs => new StudentSubjectDto
    //            {
    //                SubjectId = gs.Id
    //            }));
    //        }

    //        // ... rest of your logic here
    //    }
    //}
}
