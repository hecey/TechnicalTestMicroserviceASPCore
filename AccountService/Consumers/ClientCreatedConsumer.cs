using MassTransit;
using Hecey.TTM.ClientContracts;
using AccountService.Entities;
using AccountService.Repositories;

namespace AccountService.Consumers{
    public class ClientCreatedConsumer : IConsumer<ClientCreated>
    {
        private readonly IClientRepository<Client> _repository;

        public ClientCreatedConsumer(IClientRepository<Client> repository){
            _repository=repository;
        }

        public async Task Consume(ConsumeContext<ClientCreated> context)
        {
            var message = context.Message;
            var client = await _repository.FindAsync(x => x.Identification == message.Identification);

            if(client is not null) return;

            client= new Client{
                Id = message.Id,
                Identification = message.Identification,
                Name = message.Name,
                Status = message.Status
            };

            _repository.Add(client);

            await _repository.SaveAsync();
        }
    }
}