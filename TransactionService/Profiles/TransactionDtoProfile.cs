using AutoMapper;
using TransactionService.Entities;
using TransactionService.DTOs;

namespace TransactionService.Profiles
{
    public class TransactionDtoProfile : Profile
    {
        public TransactionDtoProfile()
        {
            //Source => Dest
            CreateMap<Transaction, TransactionDto>();
            CreateMap<AccountDto, Account>();
        }
    }
}
