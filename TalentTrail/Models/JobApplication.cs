using System.ComponentModel.DataAnnotations;
using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int SeekerId { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        [StringLength(255)]
        public string ResumePath { get; set; }

        [StringLength(1000)]
        public string CoverLetter { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; }

        [Required]
        public ApplicationStatus ApplicationStatus { get; set; }

        //Relationships
        public JobSeeker? jobSeeker { get; set; }
        public JobPost? jobPost { get; set; }

    }
}
