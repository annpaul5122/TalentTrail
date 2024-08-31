namespace TalentTrail.Models
{
    public class Recommendation
    {
        public int RecommendationId { get; set; }
        public int SeekerId { get; set; }
        public int JobId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
