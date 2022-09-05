using AutoMapper;
using ClientService.DTOs;
using Common.Entities;

namespace ClientService.Profiles
{
    public class ClientDtoProfile : Profile
    {
        public ClientDtoProfile()
        {
            //Source => Dest
            CreateMap<Client, ClientDto>();

        }
    }
}
