using AccountService.DTOs;
using AutoMapper;
using Common.Entities;

namespace TTM.Api.Profiles
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
