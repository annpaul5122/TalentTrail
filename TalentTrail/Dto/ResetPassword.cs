namespace TalentTrail.Dto
{
    public class ResetPassword
    {
        public string token {  get; set; }
        public string newPassword { get; set; }

        public string confirmPassword { get; set; }

    }
}
