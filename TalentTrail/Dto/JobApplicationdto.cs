using System;
using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class JobApplicationDto
    {
        public int ApplicationId { get; set; }
        public int SeekerId { get; set; }
        public int JobId { get; set; }
        public string? CoverLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public string? JobDescription { get; set; } // Assuming you want to include job description
        public string? SeekerName { get; set; } // Assuming you want to include seeker name
    }
}
