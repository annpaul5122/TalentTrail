using TalentTrail.Enum;

namespace TalentTrail.Dto
{
    public class EmployerCreateDto
    {
        public int UserId { get; set; }
        public JobPosition JobPosition { get; set; }
        public bool IsThirdParty { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebUrl { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyAddress { get; set; }
        public string Industry { get; set; }
    }
}
