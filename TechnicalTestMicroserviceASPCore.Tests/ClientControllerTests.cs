using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TechnicalTestMicroserviceASPCore.Controllers;
using TechnicalTestMicroserviceASPCore.Models;
using TechnicalTestMicroserviceASPCore.UnitOfWork;

namespace TechnicalTestMicroserviceASPCore.Tests
{
    public class ClientControllerTests
    {
        [Fact]
        public async void Get_Returns_a_List_of_clients()
        {
            //Arrange
            int count = 5;
            var fakeClients = A.CollectionOfDummy<Cliente>(count).AsEnumerable();
            var unitOfWork = A.Fake<IUnitOfWork>();
            A.CallTo(() => unitOfWork.Clientes.GetAll()).Returns(Task.FromResult(fakeClients));
            var controller = new ClientesController(unitOfWork);

            //Act
            var actionResult = await controller.Get();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnClientes = result != null ? result.Value as IEnumerable<Cliente> : null;
            Assert.Equal(count, returnClientes is not null ? returnClientes.Count() : 0);


        }
    }
}