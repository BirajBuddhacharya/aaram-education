namespace AaramEducation.Web.Models.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalStudents { get; set; }
    public int TotalTutors { get; set; }
    public int TotalCourses { get; set; }
    public int PublishedCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int PendingGuestbookEntries { get; set; }
}
