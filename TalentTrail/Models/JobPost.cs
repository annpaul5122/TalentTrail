using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class JobPost
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public int EmployerId { get; set; }

        [Required]
        [StringLength(50)]
        public string? JobTitle { get; set; }

        [Required]
        [StringLength(255)]
        public string? JobDescription { get; set; }

        [StringLength(255)]
        public string? JobRequirements { get; set; }

        [Required]
        [StringLength(100)]
        public string? JobLocation { get; set; }

        [Required]
        [StringLength(100)]
        public string? SalaryRange { get; set; }

        [Required]
        public EmploymentType EmploymentType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ApplicationDeadline { get; set; }  

        public DateTime? UpdatedAt { get; set; }

        //Relations
        public Employer? Employer { get; set; }
        public ICollection<JobApplication> JobApplications { get; set; }=new List<JobApplication>();
        public ICollection<Recommendation> Recommendations { get; set;} =new List<Recommendation>();

    }
}
