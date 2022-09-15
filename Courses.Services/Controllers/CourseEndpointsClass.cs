using Courses.Services.Data;
using Courses.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Courses.Services.Controllers
{
    public static class CourseEndpointsClass 
    {
        public static void MapCourseEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/Course", async (ApplicationDbContext db) =>
            {

                if (db.Courses == null)
                    return null;
                else
                    return await db.Courses.ToListAsync();

            })
            .WithName("GetAllCourses");

            routes.MapGet("/api/Course/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                var id = Guid.Parse(ID1);
                if (db.Courses == null)
                    return null;
                else
                    return await db.Courses.FindAsync(id)
                        is Course model
                            ? Results.Ok(model)
                            : Results.NotFound();
            })
            .WithName("GetCourseById");

            routes.MapPut("/api/Course/{ID1}", async (string ID1, Course course, ApplicationDbContext db) =>
            {
                var foundModel = new Course();
                var id = Guid.Parse(ID1);
                if (db.Courses == null || ID1 == null)
                    return null;
                else
                    foundModel = await db.Courses.FindAsync(id);

                if (foundModel is null)
                {
                    return Results.NotFound();
                }
                //update model properties here

                foundModel.Name = course.Name;

                try
                {
                    db.Update(foundModel);
                    await db.SaveChangesAsync();
                    return Results.Ok(foundModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Courses.Any(e => e.Id.ToString().Equals(ID1)))
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
            .WithName("UpdateCourse");

            routes.MapPost("/api/Course/", async (Course course, ApplicationDbContext db) =>
            {
                Course newCourse = new Course();
                if (db.Courses == null)
                    return null;
                else
                    newCourse.Id = Guid.NewGuid();
                newCourse.Name = course.Name;
                db.Courses.Add(newCourse);
                await db.SaveChangesAsync();
                return Results.Created($"/Courses/{newCourse.Id}", newCourse);
            })
            .WithName("CreateCourse");

            routes.MapDelete("/api/Course/{ID1}", async (string ID1, ApplicationDbContext db) =>
            {
                var id = Guid.Parse(ID1);
                if (db.Courses == null)
                    return null;
                else
                    if (await db.Courses.FindAsync(id) is Course course)
                {
                    db.Courses.Remove(course);
                    await db.SaveChangesAsync();
                    return Results.Ok(course);
                }

                return Results.NotFound();
            })
            .WithName("DeleteCourse");

        }
    }
}

