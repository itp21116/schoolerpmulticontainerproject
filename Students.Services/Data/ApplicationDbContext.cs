using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.Services.Models;

namespace Students.Services.Data
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
