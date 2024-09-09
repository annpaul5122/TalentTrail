using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TalentTrail.Models;

namespace TalentTrail.Controllers
{
    public class ResumesController : Controller
    {
        private readonly TalentTrailDbContext _context;
        private readonly string _resumeUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");

        public ResumesController(TalentTrailDbContext context)
        {
            _context = context;
        }

        // GET: Resumes
        public async Task<IActionResult> Index()
        {
            var talentTrailDbContext = _context.Resumes.Include(r => r.JobSeeker);
            return View(await talentTrailDbContext.ToListAsync());
        }

        // GET: Resumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(r => r.JobSeeker)
                .FirstOrDefaultAsync(m => m.ResumeId == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // GET: Resumes/Create
        public IActionResult Create()
        {
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education");
            return View();
        }

        // POST: Resumes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile resumeFile, [Bind("ResumeId,SeekerId,IsDefault,CreatedAt,UpdatedAt")] Resume resume)
        {
            if (ModelState.IsValid)
            {
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    var filePath = Path.Combine(_resumeUploadPath, Path.GetFileName(resumeFile.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await resumeFile.CopyToAsync(stream);
                    }
                    resume.ResumePath = filePath;
                }

                resume.CreatedAt = DateTime.UtcNow;
                resume.UpdatedAt = DateTime.UtcNow;

                _context.Add(resume);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", resume.SeekerId);
            return View(resume);
        }

        // GET: Resumes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes.FindAsync(id);
            if (resume == null)
            {
                return NotFound();
            }
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", resume.SeekerId);
            return View(resume);
        }

        // POST: Resumes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile resumeFile, [Bind("ResumeId,SeekerId,ResumePath,IsDefault,CreatedAt,UpdatedAt")] Resume resume)
        {
            if (id != resume.ResumeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (resumeFile != null && resumeFile.Length > 0)
                    {
                        var filePath = Path.Combine(_resumeUploadPath, Path.GetFileName(resumeFile.FileName));
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await resumeFile.CopyToAsync(stream);
                        }
                        resume.ResumePath = filePath;
                    }

                    resume.UpdatedAt = DateTime.UtcNow;
                    _context.Update(resume);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResumeExists(resume.ResumeId))
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
            ViewData["SeekerId"] = new SelectList(_context.JobSeekers, "SeekerId", "Education", resume.SeekerId);
            return View(resume);
        }

        // GET: Resumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resume = await _context.Resumes
                .Include(r => r.JobSeeker)
                .FirstOrDefaultAsync(m => m.ResumeId == id);
            if (resume == null)
            {
                return NotFound();
            }

            return View(resume);
        }

        // POST: Resumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resume = await _context.Resumes.FindAsync(id);
            if (resume != null)
            {
                _context.Resumes.Remove(resume);
                if (!string.IsNullOrEmpty(resume.ResumePath))
                {
                    System.IO.File.Delete(resume.ResumePath);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResumeExists(int id)
        {
            return _context.Resumes.Any(e => e.ResumeId == id);
        }
    }
}
