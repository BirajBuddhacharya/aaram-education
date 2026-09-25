using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;
using AaramEducation.Core.Interfaces;
using AaramEducation.Infrastructure.Data;

namespace AaramEducation.Infrastructure.Repositories
{
    public class GuestbookRepository : IGuestbookRepository
    {
        private readonly ApplicationDbContext _db;
        public GuestbookRepository(ApplicationDbContext db) { _db = db; }

        public async Task<IEnumerable<GuestbookEntry>> GetApprovedAsync(int page, int pageSize) =>
            await _db.GuestbookEntries.AsNoTracking()
                .Where(e => e.ModerationStatus == ModerationStatus.Approved)
                .OrderByDescending(e => e.SubmittedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync().ConfigureAwait(false);

        public async Task<IEnumerable<GuestbookEntry>> GetPendingAsync() =>
            await _db.GuestbookEntries.AsNoTracking()
                .Where(e => e.ModerationStatus == ModerationStatus.Pending)
                .OrderBy(e => e.SubmittedAt)
                .ToListAsync().ConfigureAwait(false);

        public async Task<GuestbookEntry> SubmitAsync(GuestbookEntry entry)
        {
            _db.GuestbookEntries.Add(entry);
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return entry;
        }

        public async Task ModerateAsync(int entryId, ModerationStatus status, int moderatorId)
        {
            var entry = await _db.GuestbookEntries.FindAsync(entryId).ConfigureAwait(false);
            if (entry is null) return;

            entry.ModerationStatus = status;
            entry.ModeratedBy = moderatorId;
            entry.ModeratedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int entryId)
        {
            var entry = await _db.GuestbookEntries.FindAsync(entryId).ConfigureAwait(false);
            if (entry is not null)
            {
                _db.GuestbookEntries.Remove(entry);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public Task<int> GetApprovedCountAsync() =>
            _db.GuestbookEntries.CountAsync(e => e.ModerationStatus == ModerationStatus.Approved);
    }
}
