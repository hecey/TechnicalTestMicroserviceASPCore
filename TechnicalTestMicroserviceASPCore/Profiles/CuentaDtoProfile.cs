using AutoMapper;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Profiles
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
