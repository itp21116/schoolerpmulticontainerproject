using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolERP.MVC.UI.Data;
using SchoolERP.MVC.UI.Models;
using SchoolERP.MVC.UI.Utilities;
using System.Data;

namespace SchoolERP.MVC.UI
{
    public class Startup
    {
        //private readonly ApplicationDbContext db;

        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services, IServiceProvider serviceProvider)
        {
            services.AddRazorPages();
            var context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            CreateRoles(serviceProvider, context).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider, ApplicationDbContext db)
        {

            //CREATING ROLES
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!await RoleManager.RoleExistsAsync(Roles.AdminEndUser))
            {
                await RoleManager.CreateAsync(new IdentityRole(Roles.AdminEndUser));
            }

            if (!await RoleManager.RoleExistsAsync(Roles.StudentEndUser))
            {
                await RoleManager.CreateAsync(new IdentityRole(Roles.StudentEndUser));
            }
            //CREATING USERS
            var usersExist = await db.Users.ToListAsync();

            var student1 = new ApplicationUser();
            var student2 = new ApplicationUser();

            if (usersExist.Count == 0)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "amdin@gmail.com",
                    Email = "amdin@gmail.com",
                    FirstName = "admin",
                    LastName = "admin"

                };
                admin.LockoutEnabled = false;

                var pass = UserManager.PasswordHasher.HashPassword(admin, "Admin!1!");
                admin.PasswordHash = pass;
                await saveUser(UserManager, admin, Roles.AdminEndUser, db);


                student1.UserName = "marvin@gmail.com";
                student1.Email = "marving@gmail.com";
                student1.FirstName = "Marvin";
                student1.LastName = "Lee";
                student1.DateOfBirth = Convert.ToDateTime("1995-12-25");
                student1.DateOfEnrollment = DateTime.Now;
                student1.Average = 6.5f;
                student1.IsAGraduate = false;
                student1.LockoutEnabled = false;

                pass = UserManager.PasswordHasher.HashPassword(student1, "Student!1!");
                student1.PasswordHash = pass;
                await saveUser(UserManager, student1, Roles.StudentEndUser, db);


                student2.UserName = "nick@gmail.com";
                student2.Email = "nick@gmail.com";
                student2.FirstName = "Nick";
                student2.LastName = "Miller";
                student2.DateOfBirth = Convert.ToDateTime("1997-12-31");
                student2.DateOfEnrollment = DateTime.Now;
                student2.Average = 9.5f;
                student2.IsAGraduate = true;
                student2.LockoutEnabled = false;

                pass = UserManager.PasswordHasher.HashPassword(student2, "Student!2!");
                student2.PasswordHash = pass;
                await saveUser(UserManager, student2, Roles.StudentEndUser, db);
            }

            //CREATING COURSES
            if (db.Courses is null || db.StudentsCourses is null || db.Students is null) { return; }
            var existingCourses = await db.Courses.ToListAsync();

            if (existingCourses.Count == 0)
            {
                var listOfCourses = new List<String>() { "Math", "Physics", "Literature", "Music" };
                Course course = new Course();
                foreach (string c in listOfCourses)
                {
                    course.Id = Guid.NewGuid();
                    course.Name = c;
                    await saveCourse(course, db);
                }
            }

            //CREATING STUDENTSCOURSES
            var existingStudentsCourses = await db.StudentsCourses.ToListAsync();

            if (existingStudentsCourses.Count == 0)
            {
                var studentsList = new List<ApplicationUser>();
                studentsList.Add(student1);
                studentsList.Add(student2);
                var courses = (List<Course>)await db.Courses.ToListAsync();
                var studentCourse = new StudentsCourses();

                foreach (Course c in courses)
                {
                    studentCourse.Id = Guid.NewGuid();
                    studentCourse.CourseId = c.Id;
                    studentCourse.StudentId = Guid.Parse(student1.Id);
                    studentCourse.Grade = 6.5f;
                    await saveStudentsCourses(studentCourse, db);
                    if (c.Name == "Literature") { break; }
                }

                foreach (Course c in courses)
                {
                    studentCourse.Id = Guid.NewGuid();
                    studentCourse.CourseId = c.Id;
                    studentCourse.StudentId = Guid.Parse(student2.Id);
                    studentCourse.Grade = 9.5f;
                    await saveStudentsCourses(studentCourse, db);
                }
            }


        }

        private async Task saveUser(UserManager<ApplicationUser> UserManager, ApplicationUser user, string role, ApplicationDbContext db)
        {
            await UserManager.CreateAsync(user);
            await UserManager.AddToRoleAsync(user, role);
            await db.SaveChangesAsync();
        }

        private async Task saveCourse(Course course, ApplicationDbContext db)
        {
            if (db.Courses is null) { return; }
            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();
        }

        private async Task saveStudentsCourses(StudentsCourses studentsCourses, ApplicationDbContext db)
        {
            if (db.StudentsCourses is null) { return; }
            await db.StudentsCourses.AddAsync(studentsCourses);
            await db.SaveChangesAsync();

        }

    }
}
