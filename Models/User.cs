using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string JobeRole { get; set; }
        public string PhoneNumber { get; set; }
    }
}
