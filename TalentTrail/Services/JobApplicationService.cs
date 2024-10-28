using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using TalentTrail.Dto;
using TalentTrail.Enum;
using TalentTrail.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TalentTrail.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly TalentTrailDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public JobApplicationService(TalentTrailDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<JobApplication> CreateJobApplication(ApplyJobDto applyJobDto)
        {
            var jobSeeker = await _dbContext.JobSeekers
                .Include(js => js.Resumes)
                .FirstOrDefaultAsync(js => js.SeekerId == applyJobDto.seekerId);

            if (jobSeeker == null)
            {
                throw new ArgumentException("Job Seeker not found.");
            }

            var jobPost = await _dbContext.JobPosts.FindAsync(applyJobDto.jobId);
            if (jobPost == null)
            {
                throw new ArgumentException("Job Post not found.");
            }

            if(jobPost.ApplicationDeadline < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Cannot apply for the job post.");
            }

            var resume = jobSeeker.Resumes.FirstOrDefault(r => r.ResumeId == applyJobDto.resumeId);
            if (resume == null)
            {
                throw new ArgumentException("Resume not found or does not belong to the job seeker.");
            }

            var jobApplication = new JobApplication
            {
                SeekerId = applyJobDto.seekerId,
                JobId = applyJobDto.jobId,
                CoverLetter = applyJobDto.coverLetter,
                ResumePath = resume.ResumePath,
                ApplicationDate = DateTime.UtcNow.Date,
                ApplicationStatus = ApplicationStatus.Applied
            };

            _dbContext.JobApplications.Add(jobApplication);
            await _dbContext.SaveChangesAsync();

            return jobApplication;
        }

        public async Task<JobApplicationDto> GetJobApplicationById(int applicationId)
        {
            var application = await _dbContext.JobApplications
                .Include(ja => ja.jobSeeker)
                .Include(ja => ja.jobSeeker.User)
                .FirstOrDefaultAsync(ja => ja.ApplicationId==applicationId);

            if(application == null)
            {
                throw new ArgumentException("Job Application not found.");
            }

            var applicationDto = new JobApplicationDto()
            {
                ApplicationId = applicationId,
                SeekerName = application.jobSeeker.User.FirstName + " " + application.jobSeeker.User.LastName,
                Email = application.jobSeeker.User.Email,
                JobId = application.JobId,
                CoverLetter = application.CoverLetter,
                ResumePath = application.ResumePath,
                ApplicationDate = application.ApplicationDate.Date,
                ApplicationStatus = application.ApplicationStatus.ToString()
            };

            return applicationDto;
        }


        public async Task<List<JobApplicationDto>> GetAllJobApplicationsByJobSeeker(int seekerId)
        {
            var jobApplications = await _dbContext.JobApplications
               .Include(ja => ja.jobSeeker)
               .Include(ja => ja.jobSeeker.User)
               .Where(ja => ja.SeekerId == seekerId)
               .ToListAsync();

            var jobApplicationDtos = jobApplications.Select(ja => new JobApplicationDto
            {
                ApplicationId = ja.ApplicationId,
                SeekerName = ja.jobSeeker.User.FirstName + " " + ja.jobSeeker.User.LastName,
                Email = ja.jobSeeker.User.Email,
                JobId = ja.JobId,
                CoverLetter = ja.CoverLetter,
                ResumePath = ja.ResumePath,
                ApplicationDate = ja.ApplicationDate.Date,
                ApplicationStatus = ja.ApplicationStatus.ToString()
            }).ToList();

            return jobApplicationDtos;
        }

        public async Task<List<JobApplicationDto>> GetAllJobApplicationsByJobPosts(int jobId)
        {
            var jobApplications = await _dbContext.JobApplications
               .Include(ja => ja.jobSeeker)
               .Include(ja => ja.jobSeeker.User)
               .Where(ja => ja.JobId==jobId)
               .ToListAsync();

            var jobApplicationDtos = jobApplications.Select(ja => new JobApplicationDto
            {
                ApplicationId = ja.ApplicationId,
                SeekerName = ja.jobSeeker.User.FirstName + " " + ja.jobSeeker.User.LastName,
                Email = ja.jobSeeker.User.Email,
                JobId = ja.JobId,
                CoverLetter = ja.CoverLetter,
                ResumePath = ja.ResumePath,
                ApplicationDate = ja.ApplicationDate.Date,
                ApplicationStatus = ja.ApplicationStatus.ToString()
            }).ToList();

            return jobApplicationDtos;
        }

        public async Task DeleteJobApplication(int id)
        {
            var application = await _dbContext.JobApplications.FindAsync(id);
            if(application == null)
            {
                throw new ArgumentException("Job Application not Found");
            }
            
            _dbContext.JobApplications.Remove(application);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateJobApplication(ApplyJobDto applyJobDto)
        {
            var jobApplication = await _dbContext.JobApplications
                .FirstOrDefaultAsync(ja => ja.SeekerId == applyJobDto.seekerId && ja.JobId == applyJobDto.jobId);

            if (jobApplication == null)
            {
                throw new ArgumentException("Job Application not found.");
            }

            var resume = await _dbContext.Resumes
                .FirstOrDefaultAsync(r => r.ResumeId == applyJobDto.resumeId && r.SeekerId == applyJobDto.seekerId);

            if (resume == null)
            {
                throw new ArgumentException("Invalid Resume ID.");
            }

            jobApplication.CoverLetter = applyJobDto.coverLetter;
            jobApplication.ApplicationDate = DateTime.UtcNow.Date; 
            jobApplication.ResumePath = resume.ResumePath;

            _dbContext.JobApplications.Update(jobApplication);
            await _dbContext.SaveChangesAsync();
        }

        private double DrawWrappedText(XGraphics gfx, string text, XFont font, XBrush brush, double xPosition, double yPosition, double maxWidth, double lineHeight)
        {
            string[] words = text.Split(' ');
            string line = "";

            foreach (var word in words)
            {
                string testLine = string.IsNullOrEmpty(line) ? word : line + " " + word;
                XSize size = gfx.MeasureString(testLine, font);

                if (size.Width > maxWidth)
                {
                    gfx.DrawString(line, font, brush, new XPoint(xPosition, yPosition));
                    yPosition += lineHeight;
                    line = word;
                }
                else
                {
                    line = testLine;
                }
            }

            if (!string.IsNullOrEmpty(line))
            {
                gfx.DrawString(line, font, brush, new XPoint(xPosition, yPosition));
                yPosition += lineHeight;
            }

            return yPosition;
        }
        public async Task<byte[]> GenerateApplicantPdfAsync(int applicationId)
        {
            // Fetch the applicant's data
            var applicant = await _dbContext.JobApplications
                 .Include(ja => ja.jobSeeker)
               .Include(ja => ja.jobSeeker.User)
               .SingleOrDefaultAsync(ja => ja.ApplicationId == applicationId);

            if (applicant == null)
            {
                throw new KeyNotFoundException("Applicant not found.");
            }

            // Create a PDF document
            using (var memoryStream = new MemoryStream())
            {
                PdfDocument pdf = new PdfDocument();
                var SeekerName = applicant.jobSeeker.User.FirstName + " " + applicant.jobSeeker.User.LastName;
                pdf.Info.Title = $"{SeekerName}_Profile";
                string logoPath = Path.Combine(_env.WebRootPath, "images", "logo-trail1.png");

                // Create a new page and write applicant data
                PdfPage page = pdf.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont fontRegular = new XFont("Verdana", 12, XFontStyle.Regular);
                XFont fontBold = new XFont("Verdana", 12, XFontStyle.Bold);

                double yPosition = 60;
                double lineHeight = 20; // Distance between each line
                double pageWidth = page.Width - 80; // Page width minus margins

                if (File.Exists(logoPath))
                {
                    XImage logo = XImage.FromFile(logoPath);
                    gfx.DrawImage(logo, 30, 20, 150, 70);
                }

                XPen borderPen = new XPen(XColors.Black, 1);
                gfx.DrawRectangle(borderPen, 20, 20, page.Width - 40, page.Height - 40);

               
                yPosition += 60;

                // Draw the Applicant Name heading and content
                gfx.DrawString("Applicant Name:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                double applicantNameWidth = gfx.MeasureString("Applicant Name:", fontBold).Width;
                gfx.DrawString($"{applicant.jobSeeker.User.FirstName} {applicant.jobSeeker.User.LastName}", fontRegular, XBrushes.Black, new XPoint(40 + applicantNameWidth + 10, yPosition)); // Adding spacing
                yPosition += lineHeight;
                yPosition += lineHeight;

                // Draw the Email heading and content
                gfx.DrawString("Email:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                double emailWidth = gfx.MeasureString("Email:", fontBold).Width;
                gfx.DrawString(applicant.jobSeeker.User.Email, fontRegular, XBrushes.Black, new XPoint(40 + emailWidth + 10, yPosition)); // Adding spacing
                yPosition += lineHeight;
                yPosition += lineHeight;

                // Draw the Contact Details heading and content
                gfx.DrawString("Contact Details:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                double contactDetailsWidth = gfx.MeasureString("Contact Details:", fontBold).Width;
                gfx.DrawString(applicant.jobSeeker.PhoneNumber, fontRegular, XBrushes.Black, new XPoint(40 + contactDetailsWidth + 10, yPosition)); // Adding spacing
                yPosition += lineHeight;
                yPosition += lineHeight;

                // Draw Profile Summary heading and content (without spacing adjustment)
                gfx.DrawString("Profile Summary:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                yPosition += lineHeight;
                yPosition += lineHeight;
                yPosition = DrawWrappedText(gfx, applicant.jobSeeker.ProfileSummary, fontRegular, XBrushes.Black, 40, yPosition, pageWidth, lineHeight);
                yPosition += lineHeight;

                // Draw Cover Letter heading and content (without spacing adjustment)
                gfx.DrawString("Cover Letter:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                yPosition += lineHeight;
                yPosition += lineHeight;
                yPosition = DrawWrappedText(gfx, applicant.CoverLetter ?? "N/A", fontRegular, XBrushes.Black, 40, yPosition, pageWidth, lineHeight);
                yPosition += lineHeight;

                // Draw Skills heading and content
                gfx.DrawString("Skills:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                double skillsWidth = gfx.MeasureString("Skills:", fontBold).Width;
                gfx.DrawString(applicant.jobSeeker.Skills, fontRegular, XBrushes.Black, new XPoint(40 + skillsWidth + 10, yPosition)); // Adding spacing
                yPosition += lineHeight;
                yPosition += lineHeight;

                // Draw Application Date heading and content
                gfx.DrawString("Application Date:", fontBold, XBrushes.Black, new XPoint(40, yPosition));
                double applicationDateWidth = gfx.MeasureString("Application Date:", fontBold).Width;
                gfx.DrawString(applicant.ApplicationDate.ToString("dd/MM/yyyy"), fontRegular, XBrushes.Black, new XPoint(40 + applicationDateWidth + 10, yPosition));

                // Save the PDF to memory and return it as byte array
                pdf.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }

    }
}
