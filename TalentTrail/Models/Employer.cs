using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        public int CompanyId { get; set; }



        //Relations
        public Users? Users { get; set; }
        public CompanyDetails? CompanyDetails { get; set; }
        public ICollection<JobPost> Posts { get; set; }=new List<JobPost>();


    }
}
