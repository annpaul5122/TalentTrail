using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;
using TalentTrail.Enum;

namespace TalentTrail.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly TalentTrailDbContext _context;

        public JobApplicationsController(TalentTrailDbContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index(string searchString, ApplicationStatus? statusFilter)
        {
            var talentTrailDbContext = _context.JobApplications
                .Include(j => j.jobPost)
                .Include(j => j.jobSeeker)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                talentTrailDbContext = talentTrailDbContext.Where(j => j.CoverLetter.Contains(searchString));
            }

            if (statusFilter.HasValue)
            {
                talentTrailDbContext = talentTrailDbContext.Where(j => j.ApplicationStatus == statusFilter.Value);
            }

            return View(await talentTrailDbContext.ToListAsync());
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.jobPost)
                .Include(j => j.jobSeeker)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Create
        public IActionResult Create()
        {
            ViewData["JobId"] = new SelectList(_context.JobPosts, "JobId", "JobDescription");
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education");
            ViewData["ApplicationStatuses"] = GetApplicationStatusSelectList();
            return View();
        }

        // POST: JobApplications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,SeekerId,JobId,CoverLetter,ApplicationDate,ApplicationStatus")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                if (jobApplication.ApplicationDate > DateTime.Now)
                {
                    ModelState.AddModelError(nameof(jobApplication.ApplicationDate), "Application date cannot be in the future.");
                    return View(jobApplication);
                }

                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.JobPosts, "JobId", "JobDescription", jobApplication.JobId);
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", jobApplication.SeekerId);
            ViewData["ApplicationStatuses"] = GetApplicationStatusSelectList();
            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.JobPosts, "JobId", "JobDescription", jobApplication.JobId);
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", jobApplication.SeekerId);
            ViewData["ApplicationStatuses"] = GetApplicationStatusSelectList();
            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,SeekerId,JobId,CoverLetter,ApplicationDate,ApplicationStatus")] JobApplication jobApplication)
        {
            if (id != jobApplication.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (jobApplication.ApplicationDate > DateTime.Now)
                {
                    ModelState.AddModelError(nameof(jobApplication.ApplicationDate), "Application date cannot be in the future.");
                    return View(jobApplication);
                }

                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.ApplicationId))
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
            ViewData["JobId"] = new SelectList(_context.JobPosts, "JobId", "JobDescription", jobApplication.JobId);
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", jobApplication.SeekerId);
            ViewData["ApplicationStatuses"] = GetApplicationStatusSelectList();
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.jobPost)
                .Include(j => j.jobSeeker)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.ApplicationId == id);
        }

        private List<SelectListItem> GetApplicationStatusSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = ApplicationStatus.Applied.ToString(), Text = ApplicationStatus.Applied.ToString() },
                new SelectListItem { Value = ApplicationStatus.InterviewScheduled.ToString(), Text = ApplicationStatus.InterviewScheduled.ToString() },
                new SelectListItem { Value = ApplicationStatus.OfferExtended.ToString(), Text = ApplicationStatus.OfferExtended.ToString() },
                new SelectListItem { Value = ApplicationStatus.Accepted.ToString(), Text = ApplicationStatus.Accepted.ToString() },
                new SelectListItem { Value = ApplicationStatus.Rejected.ToString(), Text = ApplicationStatus.Rejected.ToString() },
                new SelectListItem { Value = ApplicationStatus.Withdrawn.ToString(), Text = ApplicationStatus.Withdrawn.ToString() }
            };
        }
    }
}
