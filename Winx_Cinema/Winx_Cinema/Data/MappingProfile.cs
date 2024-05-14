using AutoMapper;
using Winx_Cinema.Shared.Dtos;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Film, FilmDto>().ReverseMap();
            CreateMap<Film, NewFilmDto>().ReverseMap();
            CreateMap<FilmResolution, FilmResolutionDto>().ReverseMap();
            CreateMap<FilmResolution, NewFilmResolutionDto>().ReverseMap();
            CreateMap<Hall, HallDto>().ReverseMap();
            CreateMap<Hall, NewHallDto>().ReverseMap();
            CreateMap<Session, SessionDto>().ReverseMap();
            CreateMap<Session, NewSessionDto>().ReverseMap();
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Ticket, NewTicketDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, NewUserDto>().ReverseMap();
        }
    }
}
