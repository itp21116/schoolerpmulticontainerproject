using System.ComponentModel.DataAnnotations.Schema;

namespace Students.Services.ViewModels
{
    [NotMapped]
    public class StudentGraduateVM
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public float Grade { get; set; }
        public Boolean IsGraduate { get; set; }
    }
}
