using MassTransit;
using Hecey.TTM.ClientContracts;
using Hecey.TTM.Common.Repositories;
using ClientService.DTOs;
using AccountService.Entities;

namespace AccountService.Consumers{
    public class ClientDeletedConsumer : IConsumer<ClientDeleted>
    {
        private readonly IRepository<Client> _repository;

        public ClientDeletedConsumer(IRepository<Client> repository){
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