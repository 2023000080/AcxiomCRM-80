using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcxiomCRM.Data;
using AcxiomCRM.Models;

namespace AcxiomCRM.Controllers
{
    public class FollowUpController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowUpController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FollowUp
        public async Task<IActionResult> Index()
        {
            return View(await _context.FollowUps.ToListAsync());
        }

        // GET: FollowUp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .FirstOrDefaultAsync(m => m.FollowUpId == id);
            if (followUp == null)
            {
                return NotFound();
            }

            return View(followUp);
        }

        // GET: FollowUp/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FollowUp/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowUpId,CustomerId,LeadId,OpportunityId,FollowUpDate,FollowUpType,Remarks,Status,AssignedTo")] FollowUp followUp)
        {
            if (followUp.FollowUpDate.Date < DateTime.Today)
{
    ModelState.AddModelError(
        "FollowUpDate",
        "Follow-up date cannot be earlier than today.");
}
            if (ModelState.IsValid)
            {
                _context.Add(followUp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(followUp);
        }

        // GET: FollowUp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp == null)
            {
                return NotFound();
            }
            return View(followUp);
        }

        // POST: FollowUp/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FollowUpId,CustomerId,LeadId,OpportunityId,FollowUpDate,FollowUpType,Remarks,Status,AssignedTo")] FollowUp followUp)
        {
            if (id != followUp.FollowUpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(followUp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowUpExists(followUp.FollowUpId))
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
            return View(followUp);
        }

        // GET: FollowUp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .FirstOrDefaultAsync(m => m.FollowUpId == id);
            if (followUp == null)
            {
                return NotFound();
            }

            return View(followUp);
        }

        // POST: FollowUp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var followUp = await _context.FollowUps.FindAsync(id);
            if (followUp != null)
            {
                _context.FollowUps.Remove(followUp);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowUpExists(int id)
        {
            return _context.FollowUps.Any(e => e.FollowUpId == id);
        }
    }
}
