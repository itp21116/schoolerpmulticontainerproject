using Microsoft.EntityFrameworkCore;
using Students.Services.Data;
using Students.Services.Models;
using Students.Services.ViewModels;

namespace Students.Services.Controllers
{
    public static class StudentsEndpointClass
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/Student", async (ApplicationDbContext db) =>
            {

                if (db.Students == null)
                {
                    return null;
                }

                return await db.Students.Where(s => s.FirstName != "admin").ToListAsync();


            })
            .WithName("GetAllStudents");

            routes.MapGet("/api/Student/Grades/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                if (db.StudentsCourses == null || db.Courses == null)
                {
                    return null;
                }

                var id = Guid.Parse(ID1);

                var listOfStudentCourses = new List<StudentCourseVM>();

                var studentsCourses = await db.StudentsCourses
                                        .Where(s => (s.StudentId.CompareTo(id) == 0 ? true : false)).ToListAsync();


                foreach (var s in studentsCourses)
                {
                    var coursesVM = new StudentCourseVM();
                    var course = await db.Courses.FindAsync(s.CourseId);
                    if (course == null) { continue; }
                    coursesVM.CourseName = course.Name;
                    coursesVM.Grade = s.Grade;
                    coursesVM.StudentId = id;
                    listOfStudentCourses.Add(coursesVM);
                }

                return listOfStudentCourses;


            })
            .WithName("GetGradesForAStudent");

            routes.MapPut("/api/Student/Grades/{ID1}", async (string ID1, List<SelectedCoursesVM> selectedCourses, ApplicationDbContext db) =>
            {
                if (db.StudentsCourses == null || db.Courses == null)
                {
                    return null;
                }

                var id = Guid.Parse(ID1);

                var studentsCourses = await db.StudentsCourses
                        .Where(s => (s.StudentId.CompareTo(id) == 0 ? true : false)).ToListAsync();

                var existingEnrollments = studentsCourses.Select(c => c.CourseId);

                var updatedEnrollements = selectedCourses.Select(c => c.SelectedCourseCourseId);

                foreach (var gid in existingEnrollments)
                {
                    if (!updatedEnrollements.Contains(gid))
                    {
                        var toBeRemoved = await db.StudentsCourses.Where(c => c.CourseId.Equals(gid)).FirstAsync();
                        db.StudentsCourses.Remove(toBeRemoved);
                        await db.SaveChangesAsync();
                    }
                }

                foreach (var gid in updatedEnrollements)
                {
                    var newEnrollement = new StudentsCourses();
                    if (existingEnrollments.Contains(gid))
                    {
                        continue;
                    }
                    else
                    {
                        newEnrollement.Id = Guid.NewGuid();
                        newEnrollement.StudentId = id;
                        newEnrollement.CourseId = gid;

                        db.StudentsCourses.Add(newEnrollement);
                        await db.SaveChangesAsync();
                    }
                }

                return studentsCourses;

            })
            .WithName("UpdateEnrollments");

            routes.MapGet("/api/Student/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                var id = Guid.Parse(ID1);

                if (db.Students == null)
                    return null;
                else
                    return await db.Students.FindAsync(ID1)
                        is ApplicationUser model
                            ? Results.Ok(model)
                            : Results.NotFound();
            })
            .WithName("GetStudentById");

            routes.MapGet("/api/Student/Graduate/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                ApplicationUser user = new ApplicationUser();
                if (db.Students == null)
                    return null;

                user = await db.Students.FindAsync(ID1);

                StudentGraduateVM studentGraduate = new StudentGraduateVM();
                studentGraduate.FirstName = user.FirstName;
                studentGraduate.LastName = user.LastName;
                studentGraduate.Grade = user.Average;
                studentGraduate.IsGraduate = user.IsAGraduate;

                if (studentGraduate != null)
                {
                    return studentGraduate;
                }

                return null;
            })
            .WithName("Graduated");

            routes.MapPut("/api/Student/{ID1}", async (string ID1, UpdatableStudentAdminVM student, ApplicationDbContext db) =>
            {
                var id = Guid.Parse(ID1);

                var foundModel = new ApplicationUser();

                if (db.Students == null || ID1 == null || student == null)
                    return null;
                else
                    foundModel = await db.Students.FindAsync(ID1);

                if (foundModel is null)
                {
                    return Results.NotFound();
                }

                try
                {
                    foundModel.FirstName = student.FirstName;
                    foundModel.LastName = student.LastName;
                    foundModel.Email = student.Email;
                    foundModel.DateOfBirth = student.DateOfBirth;
                    foundModel.DateOfEnrollment = student.DateOfEnrollment;
                    foundModel.IsAGraduate = student.IsAGraduate;
                    foundModel.UserName = student.Email;
                    foundModel.NormalizedEmail = student.Email.ToUpper();
                    foundModel.NormalizedUserName = student.Email.ToUpper();
                    db.Update(foundModel);
                    await db.SaveChangesAsync();
                    return Results.Ok(foundModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Students.Any(e => e.Id.Equals(ID1)))
                    {
                        return null;
                    }
                    else
                    {
                        throw;
                    }
                }

                return Results.NoContent();
            })
            .WithName("UpdateStudent");

            routes.MapPost("/api/Student/", async (UpdatableStudentAdminVM student, ApplicationDbContext db) =>
            {
                ApplicationUser newStudent = new ApplicationUser();
                if (db.Students == null || student == null)
                    return null;
                else
                    newStudent.Id = Guid.NewGuid().ToString();
                newStudent.FirstName = student.FirstName;
                newStudent.LastName = student.LastName;
                newStudent.Email = student.Email;
                newStudent.DateOfBirth = student.DateOfBirth;
                newStudent.DateOfEnrollment = student.DateOfEnrollment;
                newStudent.Average = student.Average;
                newStudent.IsAGraduate = student.IsAGraduate;
                newStudent.UserName = student.Email;
                newStudent.NormalizedEmail = student.Email.ToUpper();
                newStudent.NormalizedUserName = student.Email.ToUpper();
                db.Students.Add(newStudent);
                await db.SaveChangesAsync();
                return Results.Created($"/Students/{newStudent.Id}", newStudent);
            })
           .WithName("CreateStudent");

            routes.MapPost("/api/Student/Grade", async (List<StudentCourseVM> studentCourse, ApplicationDbContext db) =>
            {
                if (studentCourse.Count > 0)
                {
                    Guid courseId = new Guid();
                    foreach (var c in studentCourse)
                    {
                        if (db.StudentsCourses != null && db.Courses != null)
                        {
                            courseId = await db.Courses.Where(co => co.Name.Equals(c.CourseName)).Select(cou => cou.Id).FirstAsync();
                            var gradeRow = await db.StudentsCourses.Where(sc => sc.CourseId.Equals(courseId) && sc.StudentId.Equals(studentCourse[0].StudentId)).FirstAsync();
                            if (gradeRow.Grade == (float)c.Grade)
                            {
                                continue;
                            }
                            gradeRow.Grade = (float)c.Grade;
                            db.StudentsCourses.Update(gradeRow);
                            await db.SaveChangesAsync();
                        }

                    }

                    Results.Ok();

                }

            })
            .WithName("InsertGrades");

            routes.MapDelete("/api/Student/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                var id = Guid.Parse(ID1);
                if (db.Students == null)
                    return null;
                else
                    if (await db.Students.FindAsync(ID1) is ApplicationUser Student)
                {
                    db.Students.Remove(Student);
                    await db.SaveChangesAsync();
                    return Results.Ok(Student);
                }

                return Results.NotFound();
            })
            .WithName("DeleteStudent");
        }
    }
}
