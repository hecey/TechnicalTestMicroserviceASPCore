using AutoMapper;
using TTM.ClientService.DTOs;
using TTM.Common.Entities;

namespace TTM.ClientService.Profiles
{
    public class ClienteDtoProfile : Profile
    {
        public ClienteDtoProfile()
        {
            //Source => Dest
            CreateMap<Client, ClientDto>();

        }
    }
}
