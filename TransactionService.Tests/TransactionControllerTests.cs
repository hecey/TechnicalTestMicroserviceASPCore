using AutoMapper;
using Common.Entities;
using Common.Repositories;
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
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new TransactionDtoProfile());
                });

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
            var _repsitory = A.Fake<ITransactionRepository<Transaction>>();
            var remoteAccountService = A.Fake<RemoteAccountService>();


            A.CallTo(() => _repsitory.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new TransactionController(_repsitory, _configRoot!, _mapper!, remoteAccountService);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnClientes = result != null ? result.Value as IEnumerable<TransactionDto> : null;
            Assert.Equal(count, returnClientes is not null ? returnClientes.Count() : 0);


        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Transaction>(count).AsEnumerable();
            var _repsitory = A.Fake<ITransactionRepository<Transaction>>();
            var remoteAccountService = A.Fake<RemoteAccountService>();
            A.CallTo(() => _repsitory.GetAll()).Returns(Task.FromResult(fakeClients));


            var controller = new TransactionController(_repsitory, _configRoot!, _mapper!, remoteAccountService);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);


        }
    }
}