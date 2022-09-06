using AccountService.DTOs;
using AutoMapper;
using Common.Entities;

namespace AccountService.Profiles
{
    public class AccountDtoProfile : Profile
    {
        public AccountDtoProfile()
        {
            //Source => Dest
            CreateMap<Account, AccountDto>();

        }
    }
}
