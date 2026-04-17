using Application.Modules.AttendanceModule;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<AttendanceAudit, AttendanceAuditLogDto>();
        }
    }
}
