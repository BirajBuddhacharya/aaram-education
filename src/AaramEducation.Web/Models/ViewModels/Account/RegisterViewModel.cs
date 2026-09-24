using System.ComponentModel.DataAnnotations;
using AaramEducation.Core.Enums;

namespace AaramEducation.Web.Models.ViewModels.Account;

public class RegisterViewModel
{
    [Required] [MaxLength(100)] public string FirstName { get; set; } = string.Empty;
    [Required] [MaxLength(100)] public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress] [MaxLength(256)] public string Email { get; set; } = string.Empty;
    [Required] [MinLength(8)] [DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
    [Required] [DataType(DataType.Password)] [Compare("Password", ErrorMessage = "Passwords do not match.")] public string ConfirmPassword { get; set; } = string.Empty;
    [Required] public UserRole Role { get; set; } = UserRole.Student;
}
