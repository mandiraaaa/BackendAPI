using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string AssignedTo { get; set; }
        public string Priority { get; set; }
        public int Status { get; set; }
        public string sprint_id { get; set; }
    }
}
