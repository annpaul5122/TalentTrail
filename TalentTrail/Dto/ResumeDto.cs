using System;

namespace TalentTrail.Dto
{
    public class ResumeDto
    {
        public int ResumeId { get; set; }
        public int SeekerId { get; set; }
        public string ResumePath { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
