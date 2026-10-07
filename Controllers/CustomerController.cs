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
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchString, string status)
{
    var customers = _context.Customers.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchString))
    {
        customers = customers.Where(c =>
            c.CustomerCode.Contains(searchString) ||
            c.CustomerName.Contains(searchString) ||
            c.Email.Contains(searchString));
    }

    if (!string.IsNullOrWhiteSpace(status))
    {
        customers = customers.Where(c => c.Status == status);
    }

    return View(await customers.ToListAsync());
}

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerCode,CustomerName,Email,Phone,CompanyName,Address,City,State,Status,CreatedDate,CreatedBy")] Customer customer)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
{
    ModelState.AddModelError("Email", "A customer with this email already exists.");
}

if (await _context.Customers.AnyAsync(c => c.Phone == customer.Phone))
{
    ModelState.AddModelError("Phone", "A customer with this phone number already exists.");
}

if (ModelState.IsValid)
{
    _context.Add(customer);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

return View(customer);
        }
        
        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerCode,CustomerName,Email,Phone,CompanyName,Address,City,State,Status,CreatedDate,CreatedBy")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }
            if (await _context.Customers.AnyAsync(c =>
    c.Email == customer.Email &&
    c.CustomerId != customer.CustomerId))
{
    ModelState.AddModelError("Email",
        "A customer with this email already exists.");
}

if (await _context.Customers.AnyAsync(c =>
    c.Phone == customer.Phone &&
    c.CustomerId != customer.CustomerId))
{
    ModelState.AddModelError("Phone",
        "A customer with this phone number already exists.");
}

if (ModelState.IsValid)
{
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
