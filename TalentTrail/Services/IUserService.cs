using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IUserService
    {
        public Task SendPasswordResetEmail(int userId);
        public Task ResetPassword(string token, string newPassword);
        public Task DeleteUser(int userId);
        public Task<Users> UpdateUserDetails(Users users);
    }
}
