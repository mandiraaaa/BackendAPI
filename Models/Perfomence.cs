using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Perfomence
    {
        public Guid id { get; set; }
        public string Employee_name { get; set; }    
        public string Taskname { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        //public DateTime Date { get; set; }  
        public string Description { get; set; }  
    }
}
