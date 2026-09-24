using System.ComponentModel.DataAnnotations;

namespace AaramEducation.Web.Models.ViewModels.Tutor;

public class LessonFormViewModel
{
    public int LessonId { get; set; }
    public int ModuleId { get; set; }
    [Required][MaxLength(200)] public string LessonTitle { get; set; } = string.Empty;
    public string? LessonDescription { get; set; }
    [Range(1, 999)] public int SequenceOrder { get; set; } = 1;
    public bool IsFreeSample { get; set; }
    public string? VideoTitle { get; set; }
    public string? VideoUrl { get; set; }
    [Range(0, int.MaxValue)] public int DurationSeconds { get; set; }
    public string? NoteTitle { get; set; }
    public string? NoteContent { get; set; }
    public IFormFile? NoteFile { get; set; }
}
