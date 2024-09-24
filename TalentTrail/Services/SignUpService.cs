using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly TalentTrailDbContext _context;
        private readonly IEmailService _emailService;
        public SignUpService(TalentTrailDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;

        }

        public async Task<Users> SignUpUserAsync(Users user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Email is already registered.");
            }

            user.Password = user.Password;
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var subject = "Welcome to Talent Trail - Account Registration Successful";
            var body = $@"
Dear {user.FirstName},

We are excited to welcome you to Talent Trail! Your account has been successfully registered, and you are now part of a dynamic platform designed to connect talent with great opportunities.

You can now log in to your account using the email address you registered with. Here are some next steps to get started:
- Complete your profile to increase your visibility.
- Browse job postings and find the perfect match for your skills and experience.
- Keep an eye out for email notifications on new job opportunities tailored to your profile.

If you have any questions or need assistance, feel free to reach out to our support team at support@talenttrail.com. We are here to help!

Best regards,  
The Talent Trail Team

";

            try
            {
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception)
            {
                
            }

            return user;

        }
    }
}
