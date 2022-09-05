using TransactionService.DTOs;

namespace TransactionService.Clients
{
    public class RemoteAccountService
    {
        private readonly HttpClient httpClient;

        public RemoteAccountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<AccountDto> GetAccountByIdAsync(Guid AccountId)
        {
            HttpResponseMessage res = await httpClient.GetAsync($"{httpClient.BaseAddress}/Accounts/{AccountId}");
            if (res.IsSuccessStatusCode)
            {
                var client = await res.Content.ReadFromJsonAsync<AccountDto>();
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
