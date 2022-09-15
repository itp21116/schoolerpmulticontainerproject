using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Services.ViewModels
{
    [NotMapped]
    public class StudentCourseVM
    {
        public string? CourseName { get; set; }
        public float? Grade { get; set; }

        public Guid? StudentId { get; set; }

    }
}
