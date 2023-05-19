using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackendAPI.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string JobRole { get; set; }

        public DateTime JoinedDate { get; set; }

        public decimal Salary { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string Uname { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //        .Property(e => e.Salary)
        //        .HasConversion<double>(); // specify the appropriate data type for the value converter
        //}

    }
}
