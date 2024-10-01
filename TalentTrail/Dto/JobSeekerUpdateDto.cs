namespace TalentTrail.Dto
{
    public class JobSeekerUpdateDto
    {
        public string? PhoneNumber { get; set; }
        public string? ProfileSummary { get; set; }
        public string? Experience { get; set; }
        public string? Skills { get; set; }
        public string? LanguagesKnown { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<EducationUpdateDto> Educations { get; set; }
        public List<CertificationUpdateDto> Certifications { get; set; }
    }
}
