using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.Controllers;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.UnitOfWork;

namespace TechnicalTestMicroserviceASPCore.Tests
{
    public class CuentaControllerTests
    {
        [Fact]
        public async void Get_Returns_Ok_Response_List_of_cuentas_When_Data_Exist()
        {
            //Arrange
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Cuenta>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();
            var automapper = A.Fake<IMapper>();

            A.CallTo(() => unitOfWork.Cuentas.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new ClientesController(unitOfWork, automapper);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnClientes = result != null ? result.Value as IEnumerable<Cliente> : null;
            Assert.Equal(count, returnClientes is not null ? returnClientes.Count() : 0);


        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Cuenta>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();
            var automapper = A.Fake<IMapper>();

            A.CallTo(() => unitOfWork.Cuentas.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new ClientesController(unitOfWork, automapper);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);


        }
    }
}