using System.ComponentModel.DataAnnotations;

namespace TalentTrail.Dto
{
    public class UsersDto
    {
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Password { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
