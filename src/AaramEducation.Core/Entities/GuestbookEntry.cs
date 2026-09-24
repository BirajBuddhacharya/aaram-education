using System;
using AaramEducation.Core.Enums;

namespace AaramEducation.Core.Entities
{
    public class GuestbookEntry
    {
        public int EntryId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string? GuestEmail { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;
        public int? ModeratedBy { get; set; }
        public DateTime? ModeratedAt { get; set; }

        public virtual User? Moderator { get; set; }
    }
}
