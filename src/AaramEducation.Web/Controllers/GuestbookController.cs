using System.Security.Claims;
using System.Text.Encodings.Web;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Web.Models.ViewModels.Guestbook;
using Microsoft.AspNetCore.Mvc;

namespace AaramEducation.Web.Controllers;

public class GuestbookController(IGuestbookRepository guestbook) : Controller
{
    private const int PageSize = 10;

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        page = Math.Max(1, page);
        var total = await guestbook.GetApprovedCountAsync();
        var totalPages = (int)Math.Ceiling((double)total / PageSize);
        var entries = await guestbook.GetApprovedAsync(page, PageSize);

        var form = new GuestbookSubmitViewModel();
        if (User.Identity?.IsAuthenticated == true)
        {
            form.GuestName = $"{User.FindFirstValue(ClaimTypes.GivenName)} {User.FindFirstValue(ClaimTypes.Surname)}".Trim();
            form.GuestEmail = User.FindFirstValue(ClaimTypes.Email);
        }

        return View(new GuestbookIndexViewModel
        {
            Entries = entries,
            SubmitForm = form,
            CurrentPage = page,
            TotalPages = Math.Max(1, totalPages),
            TotalCount = total,
        });
    }

    [HttpPost][ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([Bind(Prefix = "SubmitForm")] GuestbookSubmitViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var total = await guestbook.GetApprovedCountAsync();
            var entries = await guestbook.GetApprovedAsync(1, PageSize);
            return View("Index", new GuestbookIndexViewModel
            {
                Entries = entries,
                SubmitForm = vm,
                CurrentPage = 1,
                TotalPages = (int)Math.Ceiling((double)total / PageSize),
                TotalCount = total,
            });
        }

        await guestbook.SubmitAsync(new GuestbookEntry
        {
            GuestName = HtmlEncoder.Default.Encode(vm.GuestName),
            GuestEmail = vm.GuestEmail,
            Message = HtmlEncoder.Default.Encode(vm.Message),
            SubmittedAt = DateTime.UtcNow,
            ModerationStatus = ModerationStatus.Pending,
        });

        TempData["Success"] = "Your message has been submitted for review. Thank you!";
        return RedirectToAction("Index");
    }
}
