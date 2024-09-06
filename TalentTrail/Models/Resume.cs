using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TalentTrail.Models
{
    public class Resume
    {
        [Key]
        public int ResumeId { get; set; }

        [Required]
        public int SeekerId { get; set; }

        [Required]
        [StringLength(255)]
        public string? ResumePath { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}

        //Relations
        public JobSeeker? JobSeeker { get; set; }

    }
}
