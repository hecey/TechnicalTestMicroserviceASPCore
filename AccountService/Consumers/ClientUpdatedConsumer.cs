using MassTransit;
using Hecey.TTM.ClientContracts;
using Hecey.TTM.Common.Repositories;
using AccountService.Entities;

namespace AccountService.Consumers{
    public class ClientUpdatedConsumer : IConsumer<ClientUpdated>
    {
        private readonly IRepository<Client> _repository;

        public ClientUpdatedConsumer(IRepository<Client> repository){
            _repository=repository;
        }

        public async Task Consume(ConsumeContext<ClientUpdated> context)
        {
            var message = context.Message;
            var client = await _repository.FindAsync(x => x.Identification == message.Identification);

            if(client is null){
                client= new Client{
                    Id = message.Id,
                    Identification = message.Identification,
                    Status = message.Status
                };
            }else{
                client.Name = message.Name;
                client.Identification = message.Identification;
                client.Status = message.Status;

                _repository.Update(client);
                await _repository.SaveAsync();
            }
        }
    }
}