namespace BackendAPI.Models
{
    public class EmployeeRequestModel
    {
        public string Name { get; set; }

        public string JobRole { get; set; }

        public DateTime JoinedDate { get; set; }

        public decimal Salary { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
