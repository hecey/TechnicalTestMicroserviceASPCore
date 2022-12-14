using ClientService.DTOs;

namespace AccountService.Clients
{
    public class RemoteClientService
    {
        private readonly HttpClient httpClient;

        public RemoteClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ClientDto> GetClientByIdAsync(string ClientIdentification)
        {
            HttpResponseMessage res = await httpClient.GetAsync($"{httpClient.BaseAddress}/Client/{ClientIdentification}");
            if (res.IsSuccessStatusCode)
            {
                var client = await res.Content.ReadFromJsonAsync<ClientDto>();
                string msg = await res.Content.ReadAsStringAsync();
                return client ?? throw new Exception(msg);
            }
            else
            {
                string msg = await res.Content.ReadAsStringAsync();
                Console.WriteLine(msg);
                throw new Exception(msg);
            }
        }
    }
}
