using System.ComponentModel.DataAnnotations;
using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class Employer
    {
        [Key]
        public int EmployerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public JobPosition JobPosition { get; set; }

        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [StringLength(255)]
        public string CompanyWebUrl { get; set; }

        [StringLength(1000)]
        public string CompanyDescription { get; set; }

        [StringLength(255)]
        public string CompanyLogo {  get; set; }

        [StringLength(255)]
        public string CompanyAddress { get; set;}

        [StringLength(255)]
        [Required]
        public string Industry { get; set; }

        //Relations
        public Users? Users { get; set; }
        public ICollection<JobPost> Posts { get; set; }=new List<JobPost>();


    }
}
