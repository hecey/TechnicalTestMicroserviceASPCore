using AutoMapper;
using TTM.Api.DTOs;
using TTM.Api.Models;

namespace TTM.Api.Profiles
{
    public class CuentaDtoProfile : Profile
    {
        public CuentaDtoProfile()
        {
            //Source => Dest
            CreateMap<Cuenta, CuentaDto>();

        }
    }
}
