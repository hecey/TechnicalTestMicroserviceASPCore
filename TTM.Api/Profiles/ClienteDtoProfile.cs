using AutoMapper;
using TTM.Api.DTOs;
using TTM.Api.Models;

namespace TTM.Api.Profiles
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
