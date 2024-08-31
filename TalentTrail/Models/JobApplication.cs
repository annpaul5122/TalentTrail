namespace TalentTrail.Models
{
    public class JobApplication
    {
        public int ApplicationId { get; set; }
        public int SeekerId { get; set; }
        public int JobId { get; set; }
        public string ResumePath { get; set; }
        public string CoverLetter { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ApplicationStatus { get; set; }
    }
}
