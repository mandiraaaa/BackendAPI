using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Sprint
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
    }
}
