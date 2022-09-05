using TransactionService.DTOs;
using AutoMapper;
using Common.Entities;

namespace TransactionService.Profiles
{
    public class TransactionDtoProfile : Profile
    {
        public TransactionDtoProfile()
        {
            //Source => Dest
            CreateMap<Transaction, TransactionDto>();
        }
    }
}
