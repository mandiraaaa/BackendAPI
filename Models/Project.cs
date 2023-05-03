using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public string Duration { get; set; }
        public int Status { get; set; }
    }
}
