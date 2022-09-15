using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Services.ViewModels
{
    [NotMapped]
    public class SelectedCoursesVM
    {
        
        public Guid SelectedCourseCourseId { get; set; }

        public string? CourseName { get; set; }

        public bool IsSelected { get; set; }
    }
}
