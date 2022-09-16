using MassTransit;
using Hecey.TTM.ClientContracts;
using Hecey.TTM.Common.Repositories;
using ClientService.DTOs;
using AccountService.Entities;

namespace AccountService.Consumers{
    public class ClientCreatedConsumer : IConsumer<ClientCreated>
    {
        private readonly IRepository<Client> _repository;

        public ClientCreatedConsumer(IRepository<Client> repository){
            _repository=repository;
        }

        public async Task Consume(ConsumeContext<ClientCreated> context)
        {
            var message = context.Message;
            var client = await _repository.FindAsync(x => x.Identification == message.Identification);

            if(client is not null) return;

            client= new Client{
                Identification = message.Identification,
                Name = message.Name
            };

            _repository.Add(client);

            await _repository.SaveAsync();
        }
    }
}