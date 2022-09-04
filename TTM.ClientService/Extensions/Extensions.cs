using TTM.ClientService.DTOs;
using TTM.Common.Entities;

public static class Extensions
{
    public static ClientDto AsDto(this Client client)
    {
        return new ClientDto(client.Id, client.Name!, client.Genre!, client.Age!, client.Identification!, client.Address!, client.Phone!, client.Password!, client.Status!);
    }
}