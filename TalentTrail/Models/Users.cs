using System.Data;

namespace TalentTrail.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        private string _role;
        public string Role 
        {
            get { return _role; }
            set
            {
                if (value == Roles.Employer || value == Roles.JobSeeker)
                {
                    _role = value;
                }
                else
                {
                    throw new ArgumentException("Invalid role value.");
                }
            }
        }
        public DateTime CreatedAt { get; set; }



    }
}
