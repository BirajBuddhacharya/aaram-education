using System.ComponentModel.DataAnnotations;

namespace AaramEducation.Web.Models.ViewModels.Tutor;

public class CourseFormViewModel
{
    public int CourseId { get; set; }
    [Required][MaxLength(200)] public string CourseName { get; set; } = string.Empty;
    [Required] public string CourseDescription { get; set; } = string.Empty;
    [Required][MaxLength(100)] public string Subject { get; set; } = string.Empty;
    [Required] public string DifficultyLevel { get; set; } = "Beginner";
    public bool IsPublished { get; set; }
}
