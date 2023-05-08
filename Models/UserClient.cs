namespace BackendAPI.Models
{
    public class UserClient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }

    }
}
