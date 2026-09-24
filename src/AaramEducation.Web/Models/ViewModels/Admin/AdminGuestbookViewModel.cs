using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Admin;

public class AdminGuestbookViewModel
{
    public IEnumerable<GuestbookEntry> Pending { get; set; } = [];
}
