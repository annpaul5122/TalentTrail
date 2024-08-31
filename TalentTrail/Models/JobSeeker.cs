using System.ComponentModel.DataAnnotations;

namespace TalentTrail.Models
{
    public class JobSeeker
    {
        [Key]
        public int SeekerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [MaxLength(1000)]
        public string ProfileSummary { get; set; }

        [Required]
        [MaxLength(255)]
        public string ResumePath { get; set; }

        [Required]
        [StringLength(255)]
        public string Education { get; set; }

        [StringLength(255)]
        public string Experience { get; set; }

        [StringLength(255)]
        [Required]
        public string Skills { get; set; }

        [StringLength(255)]
        public string Certifications { get; set; }

        [StringLength(255)]
        public string Languages { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        //Relations
        public Users User { get; set; }
        public ICollection<JobApplication> Application { get; set; }=new List<JobApplication>();
        public ICollection<Resume> Resumes { get; set; } =new List<Resume>();

    }
}
