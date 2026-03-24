using Application.Modules.DepartmentsModule.Commands.DepartmentAddCommand;
using Application.Modules.DepartmentsModule.Commands.DepartmentsEditCommand;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetAllQuery;
using Application.Modules.DepartmentsModule.Queries.DepartmentGetByIdQuery;
using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentAddResponseDto>()
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Department, DepartmentEditResponseDto>()
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name));

            CreateMap<Department, DepartmentGetAllResponseDto>()
                .ForMember(dest => dest.FacultyName, opt => opt.MapFrom(src => src.Faculty.Name))
                .ForMember(dest => dest.StudentCount, opt => opt.MapFrom(src => src.Students.Count))
                // FIX #2: was src.Students.Count — copy-paste error. Subjects, not Students.
                .ForMember(dest => dest.SubjectCount, opt => opt.MapFrom(src => src.Subjects.Count))
                .ForMember(dest => dest.GroupCount, opt => opt.MapFrom(src => src.Groups.Count));

            CreateMap<Department, DepartmentGetByIdResponseDto>()
                .ForMember(dest => dest.Faculty, opt => opt.MapFrom(src => src.Faculty))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students))
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups))
                .ForMember(dest => dest.Subjects, opt => opt.MapFrom(src => src.Subjects))
                .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherDepartments));

            // Nested mappings
            CreateMap<Faculty, DepartmentFacultyDto>();

            // FIX #3: FacultyId removed from DepartmentStudentDto — no longer on Student entity.
            // AutoMapper will map all remaining fields by convention.
            CreateMap<Student, DepartmentStudentDto>();

            CreateMap<Group, DepartmentGroupDto>();

            CreateMap<Subject, DepartmentSubjectDto>();

            // FIX #4: GroupCount and ActiveLessons removed from DepartmentTeacherDto.
            // They are [NotMapped] computed properties — ProjectTo cannot translate them to SQL.
            // Map only the stored columns that actually exist in the database.
            CreateMap<TeacherDepartment, DepartmentTeacherDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Teacher.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Teacher.FullName))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.Teacher.MobileNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Teacher.Email))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Teacher.BirthDate))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Teacher.Experience));
        }
    }
}