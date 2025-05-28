using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektKunde.Data;
using ProjektKunde.Models;


namespace ProjektKunde.Controllers;

public class HomeController : Controller
{
    private AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    
    
    
    public IActionResult Index(string? searchString)
    {
        var projects = _context.Projects
            .Include(p => p.Customer)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            string searchLower = searchString.ToLower();

            projects = projects.Where(p =>
                p.Customer != null && (
                    p.title.ToLower().Contains(searchLower) ||
                    p.description.ToLower().Contains(searchLower) ||
                    p.Customer.Company.ToLower().Contains(searchLower) ||
                    p.Customer.Adress.ToLower().Contains(searchLower)
                ));
        }

        return View(projects.ToList());
    }





    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var project = _context.Projects
            .Include(c => c.Customer)
            .FirstOrDefault(p => p.Id == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

   
    public IActionResult Create()
    {
        LoadViewBag(); 
        return View();
    }


    public void LoadViewBag()
    {
        ViewBag.Customer = new SelectList(_context.Customers, nameof(Customer.Id), nameof(Customer.Company));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {
        if (!ModelState.IsValid)
        {
            LoadViewBag();
            return View(project); 
        }

       
        if (project.Start == default)
        {
            project.Start = DateTime.Now;
        }

        _context.Projects.Add(project);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    // GET
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var project = _context.Projects.Find(id);
        if (project == null) return NotFound();

        ViewBag.Customer = new SelectList(_context.Customers, "Id", "Company", project.CustomerId);
        return View(project);
    }

// POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Project project)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customer = new SelectList(_context.Customers, "Id", "Company", project.CustomerId);
            return View(project);
        }

        _context.Projects.Update(project);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
  
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var project = _context.Projects
            .Include(p => p.Customer)
            .FirstOrDefault(p => p.Id == id);

        if (project == null) return NotFound();

        return View(project);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }


}