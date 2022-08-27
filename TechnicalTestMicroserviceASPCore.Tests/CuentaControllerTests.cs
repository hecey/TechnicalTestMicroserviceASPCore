using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.Controllers;
using TechnicalTestMicroserviceASPCore.DTOs;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.Profiles;
using TechnicalTestMicroserviceASPCore.UnitOfWork;

namespace TechnicalTestMicroserviceASPCore.Tests
{
    public class CuentaControllerTests
    {
        private static IMapper? _mapper;
        public CuentaControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CuentaDtoProfile());
                });

                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

        }

        [Fact]
        public async void Get_Returns_Ok_Response_List_of_cuentas_When_Data_Exist()
        {
            //Arrange
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Cuenta>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();


            A.CallTo(() => unitOfWork.Cuentas.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new CuentasController(unitOfWork, _mapper);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnClientes = result != null ? result.Value as IEnumerable<CuentaDto> : null;
            Assert.Equal(count, returnClientes is not null ? returnClientes.Count() : 0);


        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Cuenta>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();


            A.CallTo(() => unitOfWork.Cuentas.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new CuentasController(unitOfWork, _mapper);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);


        }
    }
}