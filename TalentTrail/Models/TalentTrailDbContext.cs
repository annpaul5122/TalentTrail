using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TalentTrail.Models
{
    public class TalentTrailDbContext : DbContext
    {
        public TalentTrailDbContext(DbContextOptions<TalentTrailDbContext> options):base(options)
        {
            
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employer - User
            modelBuilder.Entity<Employer>()
                .HasOne(e => e.Users)
                .WithOne()
                .HasForeignKey<Employer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobSeeker - User
            modelBuilder.Entity<JobSeeker>()
                .HasOne(js => js.User)
                .WithOne()
                .HasForeignKey<JobSeeker>(js => js.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //User - Employer
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Employer)
                .WithOne(e => e.Users)
                .HasForeignKey<Employer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //User - JobSeeker
            modelBuilder.Entity<Users>()
                .HasOne(u => u.JobSeeker)
                .WithOne(j => j.User)
                .HasForeignKey<JobSeeker>(j => j.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Company - Employer
            modelBuilder.Entity<Employer>()
                .HasOne(e => e.CompanyDetails)
                .WithMany(c => c.Employers)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Employer - JobPost
            modelBuilder.Entity<JobPost>()
                .HasOne(jp => jp.Employer)
                .WithMany(e => e.Posts)
                .HasForeignKey(jp => jp.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobSeeker - JobApplication
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.jobSeeker)
                .WithMany(js => js.Application)
                .HasForeignKey(ja => ja.SeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobPost - JobApplication
            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.jobPost)
                .WithMany(jp => jp.JobApplications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobSeeker - Resume
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.JobSeeker)
                .WithMany(js => js.Resumes)
                .HasForeignKey(r => r.SeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobSeeker - Recommendation
            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.JobSeeker)
                .WithMany(js => js.Recommendations)
                .HasForeignKey(r => r.SeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // JobPost - Recommendation
            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.JobPost)
                .WithMany(jp => jp.Recommendations)
                .HasForeignKey(r => r.JobId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
