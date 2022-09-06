using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TTM.Api.Controllers;
using TTM.Api.DTOs;
using TTM.Api.Models;
using TTM.Api.Profiles;
using TTM.Api.UnitOfWork;

namespace TTM.Api.Tests
{
    public class ClienteControllerTests
    {
        private static IMapper? _mapper;

        public ClienteControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new ClienteDtoProfile());
                });

                _mapper = mappingConfig.CreateMapper();

            }

        }

        [Fact]
        public async void Get_Returns_Ok_Response_List_of_clients_When_Data_Exist()
        {
            //Arrange
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Cliente>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();


            A.CallTo(() => unitOfWork.Clientes.GetAll()).Returns(Task.FromResult(fakeClients));

            var controller = new ClientesController(unitOfWork, _mapper!);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnClientes = result != null ? result.Value as IEnumerable<ClienteDto> : null;
            Assert.Equal(count, returnClientes is not null ? returnClientes.Count() : 0);


        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Cliente>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();

            A.CallTo(() => unitOfWork.Clientes.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new ClientesController(unitOfWork, _mapper!);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);


        }
    }
}