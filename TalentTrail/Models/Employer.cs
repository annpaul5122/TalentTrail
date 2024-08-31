namespace TalentTrail.Models
{
    public class Employer
    {
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyWebUrl { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLogo {  get; set; }
        public string CompanyAddress { get; set;}
        public string Industry { get; set; }
    }
}
