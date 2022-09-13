using ClientService.DTOs;


namespace AccountService.Clients
{
    public class RemoteClientService
    {
        private readonly HttpClient httpClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public RemoteClientService(HttpClient httpClient,IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClient;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ClientDto> GetClientByIdAsync(string ClientId)
        {
            HttpResponseMessage res = await httpClient.GetAsync($"{httpClient.BaseAddress}/Client/{ClientId}");
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
