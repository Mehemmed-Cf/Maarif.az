using Application.Modules.RoomsModule.Commands.RoomAddCommand;
using Application.Modules.RoomsModule.Commands.RoomEditCommand;
using Application.Modules.RoomsModule.Queries.RoomGetAllQuery;
using Application.Modules.RoomsModule.Queries.RoomGetByIdQuery;
using AutoMapper;
using Domain.Models.Entities;

namespace Application.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            // Commands → Entity
            CreateMap<RoomAddRequest, Room>();
            CreateMap<RoomEditRequest, Room>();

            // Entity → Response DTOs
            CreateMap<Room, RoomAddResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.BuildingId}-{src.Number}"));

            CreateMap<Room, RoomEditResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.BuildingId}-{src.Number}"));

            CreateMap<Room, RoomGetAllResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.BuildingId}-{src.Number}"))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor));

            CreateMap<Room, RoomGetByIdResponseDto>()
                .ForMember(dest => dest.BuildingName, opt => opt.MapFrom(src => src.Building.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.BuildingId}-{src.Number}"))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor));
        }
    }
}