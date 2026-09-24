using AaramEducation.Core.Entities;

namespace AaramEducation.Web.Models.ViewModels.Admin;

public class AdminUserListViewModel
{
    public IEnumerable<User> Users { get; set; } = [];
}
