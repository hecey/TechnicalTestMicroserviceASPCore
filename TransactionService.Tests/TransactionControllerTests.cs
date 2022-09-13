using AutoMapper;
using Hecey.TTM.Common.Entities;
using Hecey.TTM.Common.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TransactionService.Clients;
using TransactionService.Controllers;
using TransactionService.DTOs;
using TransactionService.Profiles;

namespace TransactionService.Tests
{
    public class TransactionControllerTests
    {
        private static IMapper? _mapper;
        private static IConfiguration? _configRoot;

        public TransactionControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new TransactionDtoProfile()));

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _configRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        }
        [Fact]
        public async void Get_Returns_Ok_Response_List_of_cuentas_When_Data_Exist()
        {
            //Arrange
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Transaction>(count).AsEnumerable();
            var _repository = A.Fake<ITransactionRepository<Transaction>>();
            var remoteAccountService = A.Fake<RemoteAccountService>();

            A.CallTo(() => _repository.GetAsync()).Returns(Task.FromResult(fakeClients));
            var controller = new TransactionController(_repository, _configRoot!, _mapper!, remoteAccountService);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var returnTransactions = actionResult.Result is OkObjectResult result ? result.Value as IEnumerable<TransactionDto> : null;
            Assert.Equal(count, (returnTransactions?.Count()) ?? 0);
        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            const int count = 0;
            var fakeTransaction = A.CollectionOfDummy<Transaction>(count).AsEnumerable();
            var _repository = A.Fake<ITransactionRepository<Transaction>>();
            var remoteAccountService = A.Fake<RemoteAccountService>();
            A.CallTo(() => _repository.GetAsync()).Returns(Task.FromResult(fakeTransaction));

            var controller = new TransactionController(_repository, _configRoot!, _mapper!, remoteAccountService);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);
        }
    }
}