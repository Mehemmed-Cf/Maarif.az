using Application.Modules.FacultiesModule.Commands.FacultyAddCommand;
using Application.Modules.FacultiesModule.Commands.FacultyEditCommand;
using Application.Modules.FacultiesModule.Queries.FacultyGetAllQuery;
using Application.Modules.FacultiesModule.Queries.FacultyGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;
namespace Application.Mappings
{
    public class FacultyProfile : Profile
    {
        public FacultyProfile()
        {

            CreateMap<Faculty, FacultyAddResponseDto>();

            // Mapping for editing a faculty
            CreateMap<Faculty, FacultyEditResponseDto>();


            CreateMap<Faculty, FacultyGetAllResponseDto>()
            .ForMember(dest => dest.DepartmentCount, opt => opt.MapFrom(src => src.Departments.Count));

            // 1. Map the main Faculty entity to the Response
            CreateMap<Faculty, FacultyGetByIdResponseDto>();

            // 2. Map the Department entity to the lightweight DTO
            // This automatically discards Students, Teachers, and Subjects
            CreateMap<Department, FacultyDepartmentDto>();
        }
    }
}
