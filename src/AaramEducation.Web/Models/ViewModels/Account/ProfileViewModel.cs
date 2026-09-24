using System.ComponentModel.DataAnnotations;

namespace AaramEducation.Web.Models.ViewModels.Account;

public class ProfileViewModel
{
    [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress] [MaxLength(256)] public string Email { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public IFormFile? NewProfilePicture { get; set; }
    [DataType(DataType.Password)] public string? NewPassword { get; set; }
    [DataType(DataType.Password)] [Compare("NewPassword", ErrorMessage = "Passwords do not match.")] public string? ConfirmNewPassword { get; set; }
}
