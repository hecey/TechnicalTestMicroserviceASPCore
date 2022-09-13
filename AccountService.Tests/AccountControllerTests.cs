using AccountService.Clients;
using AccountService.Controllers;
using AccountService.DTOs;
using AccountService.Profiles;
using AutoMapper;
using Hecey.TTM.Common.Entities;
using Hecey.TTM.Common.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

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
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var _repository = A.Fake<IAccountRepository<Account>>();
            var remoteClientService = A.Fake<RemoteClientService>();

            A.CallTo(() => _repository.GetAsync()).Returns(Task.FromResult(fakeClients));
            var controller = new AccountController(_repository, _mapper!, remoteClientService);

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
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Account>(count).AsEnumerable();
            var _repository = A.Fake<IAccountRepository<Account>>();
            var remoteClientService = A.Fake<RemoteClientService>();

            A.CallTo(() => _repository.GetAsync()).Returns(Task.FromResult(fakeClients));
            var controller = new AccountController(_repository, _mapper!, remoteClientService);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);
        }
    }
}