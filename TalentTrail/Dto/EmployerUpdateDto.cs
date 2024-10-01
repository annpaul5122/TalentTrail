using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class EmployerUpdateDto
    {
        public JobPosition JobPosition { get; set; }
        public bool IsThirdParty { get; set; }
        public int CompanyId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
