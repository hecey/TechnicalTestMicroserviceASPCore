using AutoMapper;
using ClientService.Controllers;
using ClientService.DTOs;
using ClientService.Profiles;
using Hecey.TTM.Common.Entities;
using Hecey.TTM.Common.Repositories;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Tests
{
    public class ClientControllerTests //: IClassFixture<ClientsController>
    {
        private readonly IMapper _mapper;

        public ClientControllerTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new ClientDtoProfile()));
                _mapper = mappingConfig.CreateMapper();
            }
        }

        [Fact]
        public async void Get_Returns_Ok_Response_List_of_clients_When_Data_Exist()
        {
            //Arrange
            const int count = 5;
            var fakeClients = A.CollectionOfDummy<Client>(count).AsEnumerable();
            var repository = A.Fake<IClientRepository<Client>>();

            A.CallTo(() => repository.GetAsync()).Returns(Task.FromResult(fakeClients));

            ClientController controller = new ClientController(repository, _mapper!);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var returnClients = actionResult.Result is OkObjectResult result ? result.Value as IEnumerable<ClientDto> : null;
            Assert.Equal(count, (returnClients?.Count()) ?? 0);
        }

        [Fact]
        public async void Get_Returns_NoContent_Response_When_Data_Not_Exist()
        {
            //Arrange
            int count = 0;
            var fakeClients = A.CollectionOfDummy<Client>(count).AsEnumerable();
            var repository = A.Fake<IClientRepository<Client>>();

            A.CallTo(() => repository.GetAsync()).Returns(Task.FromResult(fakeClients));
            var controller = new ClientController(repository, _mapper!);

            //Act
            var actionResult = await controller.GetAsync();

            //Assert
            var result = actionResult.Result;
            Assert.IsType<NoContentResult>(result);
        }
    }
}