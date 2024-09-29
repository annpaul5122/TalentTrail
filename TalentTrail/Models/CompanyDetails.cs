using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TalentTrail.Models
{
    public class CompanyDetails
    {
        [Key]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string? CompanyName { get; set; }

        [StringLength(255)]
        public string? CompanyWebUrl { get; set; }

        [StringLength(1000)]
        public string? CompanyDescription { get; set; }

        [StringLength(255)]
        public string? CompanyLogo { get; set; }

        [StringLength(255)]
        public string? CompanyAddress { get; set; }

        [StringLength(255)]
        [Required]
        public string? Industry { get; set; }

        // Navigation property for Employers
        public ICollection<Employer> Employers { get; set; } = new List<Employer>();
        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
    }
}
