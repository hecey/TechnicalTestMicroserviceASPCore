using AutoMapper;
using TTM.Api.DTOs;
using TTM.Api.Models;

namespace TTM.Api.Profiles
{
    public class MoviminetoDtoProfile : Profile
    {
        public MoviminetoDtoProfile()
        {
            //Source => Dest
            CreateMap<Movimiento, MovimientoDto>();

        }
    }
}
