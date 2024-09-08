using TalentTrail.Models;

namespace TalentTrail.Services
{
    public interface IUserService
    {
        public Task ResetPassword(int userId,string newPassword);
        public Task DeleteUser(int userId);
        public Task<Users> UpdateUserDetails(Users users);
    }
}
