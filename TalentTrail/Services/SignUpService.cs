using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;

namespace TalentTrail.Services
{
    public class SignUpService : ISignUpService
    {
        private readonly TalentTrailDbContext _context;
        private readonly IPasswordHasher<Users> _passwordHasher;
        public SignUpService(TalentTrailDbContext context,IPasswordHasher<Users> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Users> SignUpUserAsync(Users user)
        {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    throw new Exception("Email is already registered.");
                }

                // Hash the password before saving
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                user.CreatedAt = DateTime.UtcNow;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            
        }
    }
}
