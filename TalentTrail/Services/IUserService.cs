using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IUserService
    {
        public Task SendPasswordResetEmail(string email);
        public Task ResetPassword(string token, string newPassword,string confirmPwd);
        public Task DeleteUser(int userId);
        public Task<Users> UpdateUserDetails(Users users);
    }
}
