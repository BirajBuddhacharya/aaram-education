using System.ComponentModel.DataAnnotations;

namespace AaramEducation.Web.Models.ViewModels.Tutor;

public class ModuleFormViewModel
{
    public int ModuleId { get; set; }
    public int CourseId { get; set; }
    [Required][MaxLength(200)] public string ModuleName { get; set; } = string.Empty;
    public string? ModuleDescription { get; set; }
    [Range(1, 999)] public int SequenceOrder { get; set; } = 1;
}
