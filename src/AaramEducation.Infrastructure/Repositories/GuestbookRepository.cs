using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AaramEducation.Infrastructure.Repositories;

public class GuestbookRepository(ApplicationDbContext db) : IGuestbookRepository
{
    public async Task<IEnumerable<GuestbookEntry>> GetApprovedAsync(int page, int pageSize) =>
        await db.GuestbookEntries.AsNoTracking()
            .Where(e => e.ModerationStatus == ModerationStatus.Approved)
            .OrderByDescending(e => e.SubmittedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<GuestbookEntry>> GetPendingAsync() =>
        await db.GuestbookEntries.AsNoTracking()
            .Where(e => e.ModerationStatus == ModerationStatus.Pending)
            .OrderBy(e => e.SubmittedAt)
            .ToListAsync();

    public async Task<GuestbookEntry> SubmitAsync(GuestbookEntry entry)
    {
        db.GuestbookEntries.Add(entry);
        await db.SaveChangesAsync();
        return entry;
    }

    public async Task ModerateAsync(int entryId, ModerationStatus status, int moderatorId)
    {
        var entry = await db.GuestbookEntries.FindAsync(entryId);
        if (entry is null) return;

        entry.ModerationStatus = status;
        entry.ModeratedBy = moderatorId;
        entry.ModeratedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int entryId)
    {
        var entry = await db.GuestbookEntries.FindAsync(entryId);
        if (entry is not null)
        {
            db.GuestbookEntries.Remove(entry);
            await db.SaveChangesAsync();
        }
    }

    public Task<int> GetApprovedCountAsync() =>
        db.GuestbookEntries.CountAsync(e => e.ModerationStatus == ModerationStatus.Approved);
}
