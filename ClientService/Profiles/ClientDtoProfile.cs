using AutoMapper;
using ClientService.DTOs;
using ClientService.Entities;

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
