using AccountService.DTOs;
using AutoMapper;
using ClientService.DTOs;
using AccountService.Entities;

namespace AccountService.Profiles
{
    public class AccountDtoProfile : Profile
    {
        public AccountDtoProfile()
        {
            //Source => Dest
            CreateMap<Account, AccountDto>();
            CreateMap<ClientDto, Client>();
        }
    }
}
