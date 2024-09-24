using System;
using System.ComponentModel.DataAnnotations;
using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class JobApplicationDto
    {
        public int ApplicationId { get; set; }
        public string? SeekerName { get; set; }
        public int JobId { get; set; }
        public string? CoverLetter { get; set; }
        public string ResumePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; }
        public string ApplicationStatus { get; set; }
        
    }
}
