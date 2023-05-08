using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Billing
    {
        [Key]
        public Guid Id { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string installmentName { get; set; }
        public double Amount { get; set; }
        
    }
}
