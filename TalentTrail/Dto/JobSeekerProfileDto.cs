namespace TalentTrail.Dto
{
    public class JobSeekerProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileSummary { get; set; }
        public string ResumePath { get; set; }
        public string Education { get; set; }
        public string? Experience { get; set; }
        public string Skills { get; set; }
        public string? Certifications { get; set; }
        public string? LanguagesKnown { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
