using System.ComponentModel.DataAnnotations;

namespace TalentTrail.Models
{
    public class Certification
    {
        [Key]
        public int CertificationId { get; set; }

        [Required]
        [StringLength(255)]
        public string CertificationName { get; set; }

        [StringLength(500)]
        public string? CertificateImagePath { get; set; }

        [Required]
        public DateTime DateIssued { get; set; }

        public int SeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
    }
}
