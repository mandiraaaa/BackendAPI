using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Billing
    {
        [Key]
        public Guid Id { get; set; }
        public Guid customerId { get; set; }
        public double price { get; set; }
        public double paidAmount { get; set; }
        public double tax { get; set; }
    }
}
