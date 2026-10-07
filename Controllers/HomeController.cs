using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcxiomCRM.Data;
using AcxiomCRM.Models;

namespace AcxiomCRM.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // =========================
        // DASHBOARD COUNTS
        // =========================

        ViewBag.TotalCustomers =
            await _context.Customers.CountAsync();

        ViewBag.TotalLeads =
            await _context.Leads.CountAsync();

        ViewBag.OpenLeads =
            await _context.Leads.CountAsync(l =>
                l.Status != "Lost" &&
                l.Status != "Converted");

        ViewBag.TotalOpportunities =
            await _context.Opportunities.CountAsync();

        ViewBag.OpenOpportunities =
            await _context.Opportunities.CountAsync(o =>
                o.Status == "Active" &&
                o.Stage != "Won" &&
                o.Stage != "Lost");

        ViewBag.WonOpportunities =
            await _context.Opportunities.CountAsync(o =>
                o.Stage == "Won");

        ViewBag.LostOpportunities =
            await _context.Opportunities.CountAsync(o =>
                o.Stage == "Lost");

        ViewBag.TotalPipeline =
            await _context.Opportunities
                .Where(o =>
                    o.Status == "Active" &&
                    o.Stage != "Won" &&
                    o.Stage != "Lost")
                .SumAsync(o => (decimal?)o.Amount) ?? 0;

        // =========================
        // LEAD STATUS CHART
        // =========================

        var leadStatusData =
            await _context.Leads
                .GroupBy(l => l.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

        ViewBag.LeadStatusLabels =
            JsonSerializer.Serialize(
                leadStatusData.Select(x => x.Status));

        ViewBag.LeadStatusValues =
            JsonSerializer.Serialize(
                leadStatusData.Select(x => x.Count));

        // =========================
        // OPPORTUNITY PIPELINE CHART
        // =========================

        var opportunityPipelineData =
            await _context.Opportunities
                .GroupBy(o => o.Stage)
                .Select(g => new
                {
                    Stage = g.Key,
                    Amount = g.Sum(o => o.Amount)
                })
                .ToListAsync();

        ViewBag.OpportunityStageLabels =
            JsonSerializer.Serialize(
                opportunityPipelineData.Select(x => x.Stage));

        ViewBag.OpportunityStageValues =
            JsonSerializer.Serialize(
                opportunityPipelineData.Select(x => x.Amount));

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId =
                System.Diagnostics.Activity.Current?.Id ??
                HttpContext.TraceIdentifier
        });
    }
}