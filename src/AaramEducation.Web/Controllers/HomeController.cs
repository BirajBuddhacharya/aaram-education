using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AaramEducation.Web.Models;

namespace AaramEducation.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Route("Home/Error/{statusCode?}")]
    public IActionResult Error(int? statusCode)
    {
        var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
        if (statusCode == 404) ViewData["StatusMessage"] = "Page not found.";
        else if (statusCode == 500) ViewData["StatusMessage"] = "Server error.";
        return View(model);
    }
}
