using AutoMapper;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Profiles
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
