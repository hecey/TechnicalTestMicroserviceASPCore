using MassTransit;
using Hecey.TTM.ClientContracts;
using ClientService.DTOs;
using AccountService.Entities;
using AccountService.Repositories;

namespace AccountService.Consumers{
    public class ClientDeletedConsumer : IConsumer<ClientDeleted>
    {
        private readonly IClientRepository<Client> _repository;

        public ClientDeletedConsumer(IClientRepository<Client> repository){
            _repository=repository;
        }

        public async Task Consume(ConsumeContext<ClientDeleted> context)
        {
            var message = context.Message;
            var client = await _repository.FindAsync(x => x.Identification == message.Identification);

            if(client is null || client.Id.Equals("")) return;

            _repository.Delete(client.Id);
            await _repository.SaveAsync();
        }
    }
}