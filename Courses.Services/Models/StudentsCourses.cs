using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Services.Models
{
    [Table("StudentsCourses")]
    public class StudentsCourses
    {
        [Key]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid CourseId { get; set; }

        public float Grade { get; set; }

        public ICollection<ApplicationUser>? Students { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}
