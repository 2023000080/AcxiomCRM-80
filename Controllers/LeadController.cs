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
    public class LeadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lead
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leads.ToListAsync());
        }

        // GET: Lead/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads
                .FirstOrDefaultAsync(m => m.LeadId == id);
            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // GET: Lead/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lead/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeadId,LeadCode,LeadName,Email,Phone,CompanyName,Source,Status,ExpectedValue,CreatedDate,AssignedTo")] Lead lead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lead);
        }

        // GET: Lead/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads.FindAsync(id);
            if (lead == null)
            {
                return NotFound();
            }
            return View(lead);
        }

        // POST: Lead/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeadId,LeadCode,LeadName,Email,Phone,CompanyName,Source,Status,ExpectedValue,CreatedDate,AssignedTo")] Lead lead)
        {
            if (id != lead.LeadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadExists(lead.LeadId))
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
            return View(lead);
        }

        // GET: Lead/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lead = await _context.Leads
                .FirstOrDefaultAsync(m => m.LeadId == id);
            if (lead == null)
            {
                return NotFound();
            }

            return View(lead);
        }

        // POST: Lead/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead != null)
            {
                _context.Leads.Remove(lead);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeadExists(int id)
        {
            return _context.Leads.Any(e => e.LeadId == id);
        }
        public async Task<IActionResult> Convert(int id)
{
    var lead = await _context.Leads.FindAsync(id);

    if (lead == null)
    {
        return NotFound();
    }

    if (lead.Status != "Qualified")
    {
        TempData["Error"] = "Only qualified leads can be converted.";
        return RedirectToAction(nameof(Index));
    }

    var customer = new Customer
    {
        CustomerCode = "CUS-" + lead.LeadCode,
        CustomerName = lead.LeadName,
        Email = lead.Email,
        Phone = lead.Phone,
        CompanyName = lead.CompanyName,
        Status = "Active",
        CreatedDate = DateTime.UtcNow,
        CreatedBy = lead.AssignedTo
    };

    _context.Customers.Add(customer);

    await _context.SaveChangesAsync();

    var opportunity = new Opportunity
    {
        OpportunityName = lead.LeadName,
        CustomerId = customer.CustomerId,
        LeadId = lead.LeadId,
        Amount = lead.ExpectedValue,
        Stage = "Qualification",
        Probability = 10,
        ExpectedCloseDate = DateTime.Today.AddDays(30),
        Status = "Active",
        CreatedDate = DateTime.UtcNow,
        AssignedTo = lead.AssignedTo
    };

    _context.Opportunities.Add(opportunity);

    lead.Status = "Converted";

    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
}
    }
}
