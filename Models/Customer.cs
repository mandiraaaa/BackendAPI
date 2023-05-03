using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string project { get; set; }
        public string Duration { get; set; }
        public string Package { get; set; }
        public string PhoneNumber { get; set; }
        public int status { get; set; }
    }
}
