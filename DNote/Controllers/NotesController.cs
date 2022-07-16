using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DNote.Data;
using DNote.Models;
using Microsoft.AspNetCore.Authorization;

namespace DNote.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = new string[1000];
            foreach(var cat in _context.Category.ToList())
            {
                ViewBag.Categories[cat.id] = cat.CategoryName;
            }
            return _context.Note != null ? 
                          View(await _context.Note.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Note'  is null.");
        }
        // GET: Notes/Search
        public async Task<IActionResult> Search()
        {
            return View();
        }
        // GET: Notes/List
        public async Task<IActionResult> List(int? id)
        {
            ViewBag.Categories = new string[1000];
            foreach (var cat in _context.Category.ToList())
            {
                ViewBag.Categories[cat.id] = cat.CategoryName;
            }
            if (id == null)
                return View("Index");
            return _context.Note != null ?
                          View("Index", await _context.Note.Where(i => i.CategoryId == id).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Note'  is null.");
        }
        // POST: Notes/Find
        [HttpPost]
        public async Task<IActionResult> Find(string SearchPhrase)
        {
            ViewBag.Categories = new string[1000];
            foreach (var cat in _context.Category.ToList())
            {
                ViewBag.Categories[cat.id] = cat.CategoryName;
            }
            return _context.Note != null ?
                          View("Index", await _context.Note.Where(n => ( n.Author == User.Identity.Name && n.Name.Contains(SearchPhrase))).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Note'  is null.");
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Note == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = GetCategoriesList().Find(i => i.id == note.CategoryId).CategoryName;
            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            ViewBag.ListOfCategories = GetCategoriesList();
            return View();
        }
        private List<Category> GetCategoriesList(Note? note = null)
        {
            var categoryList = new List<Category>();
            foreach (var cat in _context.Category)
            {
                categoryList.Insert(0, new Category { id = cat.id, CategoryName = cat.CategoryName });
            }
            categoryList.Insert(0, new Category { id = 0, CategoryName = "Select category" });
            if(!Equals(null, note))
                categoryList.Insert(0, new Category { id = note.CategoryId, CategoryName = _context.Category.Where(cat => cat.id == note.CategoryId).Select(cat => cat.CategoryName).SingleOrDefault() });
            return categoryList;
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content,CategoryId,Author")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Note == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewBag.ListOfCategories = GetCategoriesList(note);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content,CategoryId,Author")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Note == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Note == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Note'  is null.");
            }
            var note = await _context.Note.FindAsync(id);
            if (note != null)
            {
                _context.Note.Remove(note);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
          return (_context.Note?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
