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
    public class OpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpportunityController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Opportunity
        public async Task<IActionResult> Index()
        {
            return View(await _context.Opportunities.ToListAsync());
        }

        // GET: Opportunity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.OpportunityId == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // GET: Opportunity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Opportunity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OpportunityId,OpportunityName,CustomerId,LeadId,Amount,Stage,Probability,ExpectedCloseDate,Status,CreatedDate,AssignedTo,Source,Notes")] Opportunity opportunity)
        {
            if (opportunity.Status == "Active" && opportunity.Amount <= 0)
{
    ModelState.AddModelError("Amount",
        "Amount must be greater than zero for an active opportunity.");
}

if (opportunity.Probability < 0 || opportunity.Probability > 100)
{
    ModelState.AddModelError("Probability",
        "Probability must be between 0 and 100.");
}

if (opportunity.Status == "Active" &&
    opportunity.ExpectedCloseDate.Date < DateTime.Today)
{
    ModelState.AddModelError("ExpectedCloseDate",
        "Expected close date cannot be in the past for an active opportunity.");
}
            if (ModelState.IsValid)
            {
                _context.Add(opportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(opportunity);
        }

        // GET: Opportunity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null)
            {
                return NotFound();
            }
            return View(opportunity);
        }

        // POST: Opportunity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OpportunityId,OpportunityName,CustomerId,LeadId,Amount,Stage,Probability,ExpectedCloseDate,Status,CreatedDate,AssignedTo,Source,Notes")] Opportunity opportunity)
        {
            if (id != opportunity.OpportunityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opportunity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpportunityExists(opportunity.OpportunityId))
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
            return View(opportunity);
        }

        // GET: Opportunity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opportunity = await _context.Opportunities
                .FirstOrDefaultAsync(m => m.OpportunityId == id);
            if (opportunity == null)
            {
                return NotFound();
            }

            return View(opportunity);
        }

        // POST: Opportunity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity != null)
            {
                _context.Opportunities.Remove(opportunity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpportunityExists(int id)
        {
            return _context.Opportunities.Any(e => e.OpportunityId == id);
        }
    }
}
