using AutoMapper;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;

namespace TechnicalTestMicroserviceASPCore.Profiles
{
    public class ClienteDtoProfile : Profile
    {
        public ClienteDtoProfile()
        {
            //Source => Dest
            CreateMap<Cliente, ClienteDto>();

        }
    }
}
