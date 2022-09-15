using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolERP.MVC.UI.Models;

namespace SchoolERP.MVC.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser>? Students { get; set; }
        public DbSet<Course>? Courses { get; set; }
        public DbSet<StudentsCourses>? StudentsCourses { get; set; }
    }
}
