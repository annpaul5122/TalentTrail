using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface ISignUpService
    {
        Task<Users> SignUpUserAsync(Users user);
    }
}
