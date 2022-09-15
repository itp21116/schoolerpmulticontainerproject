using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolERP.MVC.UI.ViewModels
{
    [NotMapped]
    public class StudentCourseVM
    {
        public string? CourseName { get; set; }
        public float? Grade { get; set; }

        public Guid? StudentId { get; set; }

    }
}
