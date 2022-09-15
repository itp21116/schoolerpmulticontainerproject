using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Services.ViewModels
{
    [NotMapped]
    public class UpdatableStudentAdminVM
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfEnrollment { get; set; }
        public float Average { get; set; }
        public bool IsAGraduate { get; set; }
        [Email]
        public string? Email { get; set; }
    }
}
