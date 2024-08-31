using System.ComponentModel.DataAnnotations;
using System.Data;
using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MinLength(8),MaxLength(25)]
        public string Password { get; set; }

        [Required]
        
        private string _role;
        public string Role 
        {
            get { return _role; }
            set
            {
                if (value == Roles.Employer || value == Roles.JobSeeker)
                {
                    _role = value;
                }
                else
                {
                    throw new ArgumentException("Invalid role value.");
                }
            }
        }

        [Required]
        public DateTime CreatedAt { get; set; }

        //Relations
        public Employer Employer { get; set; }
        public JobSeeker JobSeeker { get; set; }


    }
}
