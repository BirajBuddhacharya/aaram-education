using AaramEducation.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AaramEducation.Web.Models.ViewModels.Guestbook;

public class GuestbookSubmitViewModel
{
    [Required][MaxLength(100)] public string GuestName { get; set; } = string.Empty;
    [EmailAddress][MaxLength(256)] public string? GuestEmail { get; set; }
    [Required][MaxLength(1000)] public string Message { get; set; } = string.Empty;
}

public class GuestbookIndexViewModel
{
    public IEnumerable<GuestbookEntry> Entries { get; set; } = [];
    public GuestbookSubmitViewModel SubmitForm { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int TotalCount { get; set; }
}
