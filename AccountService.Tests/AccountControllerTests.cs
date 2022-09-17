using AccountService.Clients;
using AccountService.Controllers;
using AccountService.DTOs;
using AccountService.Profiles;
using AutoMapper;
using AccountService.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using AccountService.Repositories;

namespace AccountService.Tests
{
    public class AccountControllerTests
    {
        private static IMapper? _mapper;
        public AccountControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new AccountDtoProfile()));
                _mapper = mappingConfig.CreateMapper();
            }
        }

        [Fact]
        public async void Get_Returns_Ok_Response_List_of_Accounts_When_Data_Exist()
        {
            //Arrange
            const int count = 5;
            var fakeAccount = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var fakeClients = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var _accountRepository = A.Fake<IAccountRepository<Account>>();
            var _clientRepository = A.Fake<IAccountRepository<Client>>();

            A.CallTo(() => _accountRepository.GetAsync()).Returns(Task.FromResult(fakeAccount));
            var controller = new AccountController(_accountRepository, _mapper!, _clientRepository);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var returnAccounts = actionResult.Result is OkObjectResult result ? result.Value as IEnumerable<AccountDto> : null;
            Assert.Equal(count, returnAccounts!.Count());
        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
             const int count = 0;
            var fakeAccount = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var fakeClients = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var _accountRepository = A.Fake<IAccountRepository<Account>>();
            var _clientRepository = A.Fake<IAccountRepository<Client>>();

            A.CallTo(() => _accountRepository.GetAsync()).Returns(Task.FromResult(fakeAccount));
            var controller = new AccountController(_accountRepository, _mapper!, _clientRepository);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);
        }
    }
}