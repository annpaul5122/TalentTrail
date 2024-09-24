using System.ComponentModel.DataAnnotations;

namespace TalentTrail.Models
{
    public class Education
    {
        [Key]
        public int EducationId { get; set; }

        [Required]
        [StringLength(255)]
        public string Degree { get; set; }

        [Required]
        [StringLength(255)]
        public string Institution { get; set; }

        [Required]
        public int PassoutYear { get; set; }

        public int SeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
    }
}
