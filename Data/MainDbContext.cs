using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Data
{
    public class MainDbContext: DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }

        //dbset
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Perfomence> perfomences { get; set; }
        public DbSet<UserClient> UserClients { get; set; }

    }
}
