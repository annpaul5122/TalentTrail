using System.ComponentModel.DataAnnotations;
using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class UpdateApplicationStatusDto
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public ApplicationStatus NewStatus { get; set; }
    }
}
