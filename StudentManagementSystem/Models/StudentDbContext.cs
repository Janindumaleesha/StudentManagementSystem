using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Models
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var ConnectingString = "Data Source=DESKTOP-I46N7EU\\SQLEXPRESS;Initial Catalog=StudentDb;Integrated Security=True;Trust Server Certificate=True";
            optionsBuilder.UseSqlServer(ConnectingString);
        }
    }
}
