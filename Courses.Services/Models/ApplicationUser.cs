using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Services.Models
{
    [Table("Students")]
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateOfEnrollment { get; set; }
        public float Average { get; set; }
        public bool IsAGraduate { get; set; }
    }
}
