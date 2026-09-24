using System.Collections.Generic;
using System.Threading.Tasks;
using AaramEducation.Core.Entities;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Interfaces
{
    public interface IGuestbookRepository
    {
        Task<IEnumerable<GuestbookEntry>> GetApprovedAsync(int page, int pageSize);
        Task<IEnumerable<GuestbookEntry>> GetPendingAsync();
        Task<GuestbookEntry> SubmitAsync(GuestbookEntry entry);
        Task ModerateAsync(int entryId, ModerationStatus status, int moderatorId);
        Task DeleteAsync(int entryId);
        Task<int> GetApprovedCountAsync();
    }
}
