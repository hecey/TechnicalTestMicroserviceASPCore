namespace ClientService.Entities
{
    public class Client : Person
    {
        public string? Password { get; set; }
        public bool Status { get; set; }
    }
}
