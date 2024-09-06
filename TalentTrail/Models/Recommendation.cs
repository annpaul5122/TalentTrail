using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TalentTrail.Models
{
    public class Recommendation
    {
        [Key]
        public int RecommendationId { get; set; }

        [Required]
        public int SeekerId { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        //Relations
        public JobSeeker? JobSeeker { get; set; }
        public JobPost? JobPost { get; set; }


    }
}
