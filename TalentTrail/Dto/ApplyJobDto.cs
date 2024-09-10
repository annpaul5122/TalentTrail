using System.ComponentModel.DataAnnotations;

namespace TalentTrail.Dto
{
    public class ApplyJobDto
    {
        [Required]
        public int seekerId {  get; set; }

        [Required]
        public int jobId { get; set; }

        [Required]
        public int resumeId { get; set; }

        [MaxLength(1000)]
        public string? coverLetter { get; set; }
    }
}
