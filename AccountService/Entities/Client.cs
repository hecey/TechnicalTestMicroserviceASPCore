namespace AccountService.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Identification { get; set; }
        public bool Status { get; set; }
    }
}
