using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class UserService : IUserService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly PasswordHasher<Users> _passwordHasher;

        public UserService(TalentTrailDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _passwordHasher = new PasswordHasher<Users>();

        }

        public async Task SendPasswordResetEmail(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new InvalidOperationException("User email is not set.");
            }

            // Generate token
            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour
            await _dbContext.SaveChangesAsync();

            // Send email
            var resetLink = $"https://localhost:7119/api/Users/reset-password?token={token}"; // Adjust URL as needed
            var subject = "Password Reset Request";
            var body = $"Please reset your password by clicking on the following link: <a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task ResetPassword(string token, string newPassword)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.PasswordResetToken == token && u.PasswordResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                throw new ArgumentException("Invalid or expired token.");
            }

            user.Password = newPassword;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if(user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var jobSeeker = await _dbContext.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);

            if (jobSeeker != null)
            {
                _dbContext.JobSeekers.Remove(jobSeeker);
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Users> UpdateUserDetails(Users users)
        {
            var user = await _dbContext.Users.FindAsync(users.UserId);
            if(user==null)
            {
                throw new ArgumentException("User not found");
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
