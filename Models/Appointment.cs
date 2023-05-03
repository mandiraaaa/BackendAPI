using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public string AppointmentName { get; set; }
        public int ParentAppoinmentId { get; set; }
    }
}
