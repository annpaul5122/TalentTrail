namespace TalentTrail.Models
{
    public class JobSeeker
    {
        public int SeekerId { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileSummary { get; set; }
        public string ResumePath { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string certfictions { get; set; }
        public string Languages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

    }
}
