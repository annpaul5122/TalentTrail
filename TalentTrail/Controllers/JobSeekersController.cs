using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TalentTrail.Controllers
{
    public class JobSeekersController : Controller
    {
        private readonly TalentTrailDbContext _context;

        public JobSeekersController(TalentTrailDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var talentTrailDbContext = _context.JobSeekers
                                                .Include(j => j.User)
                                                .Include(j => j.Application)
                                                .Include(j => j.Resumes)
                                                .Include(j => j.Recommendations)
                                                .AsQueryable(); // Convert to IQueryable here

            if (!string.IsNullOrEmpty(searchString))
            {
                talentTrailDbContext = talentTrailDbContext.Where(j => j.Skills.Contains(searchString) || j.Education.Contains(searchString));
            }

            return View(await talentTrailDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSeeker = await _context.JobSeekers
                .Include(j => j.User)
                .Include(j => j.Application)
                .Include(j => j.Resumes)
                .Include(j => j.Recommendations)
                .FirstOrDefaultAsync(m => m.SeekerId == id);
            if (jobSeeker == null)
            {
                return NotFound();
            }

            return View(jobSeeker);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeekerId,UserId,PhoneNumber,ProfileSummary,ResumePath,Education,Experience,Skills,Certifications,LanguagesKnown,CreatedAt,LastUpdatedAt")] JobSeeker jobSeeker, IFormFile resumeFile)
        {
            if (ModelState.IsValid)
            {
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resumes", resumeFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resumeFile.CopyToAsync(stream);
                    }
                    jobSeeker.ResumePath = "/resumes/" + resumeFile.FileName;
                }

                jobSeeker.CreatedAt = DateTime.Now;
                jobSeeker.LastUpdatedAt = DateTime.Now;

                _context.Add(jobSeeker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", jobSeeker.UserId);
            return View(jobSeeker);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", jobSeeker.UserId);
            return View(jobSeeker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeekerId,UserId,PhoneNumber,ProfileSummary,ResumePath,Education,Experience,Skills,Certifications,LanguagesKnown,CreatedAt,LastUpdatedAt")] JobSeeker jobSeeker, IFormFile resumeFile)
        {
            if (id != jobSeeker.SeekerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (resumeFile != null && resumeFile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/resumes", resumeFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await resumeFile.CopyToAsync(stream);
                        }
                        jobSeeker.ResumePath = "/resumes/" + resumeFile.FileName;
                    }

                    jobSeeker.LastUpdatedAt = DateTime.Now;

                    _context.Update(jobSeeker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobSeekerExists(jobSeeker.SeekerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", jobSeeker.UserId);
            return View(jobSeeker);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSeeker = await _context.JobSeekers
                .Include(j => j.User)
                .Include(j => j.Application)
                .Include(j => j.Resumes)
                .Include(j => j.Recommendations)
                .FirstOrDefaultAsync(m => m.SeekerId == id);
            if (jobSeeker == null)
            {
                return NotFound();
            }

            return View(jobSeeker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker != null)
            {
                _context.JobSeekers.Remove(jobSeeker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobSeekerExists(int id)
        {
            return _context.JobSeekers.Any(e => e.SeekerId == id);
        }
    }
}
