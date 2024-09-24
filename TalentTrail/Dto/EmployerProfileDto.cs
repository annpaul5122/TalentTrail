namespace TalentTrail.Dto
{
    public class EmployerProfileDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string JobPosition { get; set; }

        public bool IsThirdParty { get; set; }

        public CompanyDetailsDto? CompanyDetails { get; set; }
    }
}
