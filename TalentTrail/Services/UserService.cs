using Microsoft.AspNetCore.Identity;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class UserService : IUserService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IPasswordHasher<Users> _passwordHasher;
        public UserService(TalentTrailDbContext dbContext,IPasswordHasher<Users> passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task ResetPassword(int userId,string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if(user == null)
            {
                throw new Exception("User not found.");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, newPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                throw new Exception("New password cannot be the same as the old password.");
            }

            user.Password = _passwordHasher.HashPassword(user, newPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if(user == null)
            {
                throw new Exception("User not found.");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Users> UpdateUserDetails(Users users)
        {
            var user = await _dbContext.Users.FindAsync(users.UserId);
            if(user==null)
            {
                throw new Exception("User not found");
            }
            user.FirstName = users.FirstName;
            user.LastName = users.LastName;
            user.Email = users.Email;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
