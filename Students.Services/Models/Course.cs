using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Services.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
