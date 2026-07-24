using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.MVC.Data;
using LibraryManagement.MVC.Models;
using LibraryManagement.MVC.ViewModels;

namespace LibraryManagement.MVC.Controllers
{
    public class FinesController : Controller
    {
        private readonly LibraryDbContext _context;

        public FinesController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Fines
        public async Task<IActionResult> Index(string searchQuery, string filterStatus, int page = 1)
        {
            var query = _context.Fines
                .Include(f => f.Student)
                .Include(f => f.BorrowRecord)
                    .ThenInclude(b => b.Book)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(f => 
                    f.Student.Name.Contains(searchQuery) ||
                    f.BorrowRecord.Book.Title.Contains(searchQuery)
                );
            }

            if (!string.IsNullOrEmpty(filterStatus) && filterStatus != "all")
            {
                bool isPaid = filterStatus == "paid";
                query = query.Where(f => f.IsPaid == isPaid);
            }

            int pageSize = 5;
            int totalRecords = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var paged = await query
                .OrderByDescending(f => f.DueDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new FineIndexViewModel
            {
                Fines = paged,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery,
                FilterStatus = filterStatus
            };

            return View(viewModel);
        }

        // GET: Fines/Create
        public IActionResult Create()
        {
            ViewData["BorrowRecordId"] = new SelectList(_context.BorrowRecords.Include(b => b.Book), "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Fines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fine fine)
        {
            ModelState.Remove("Id");
            ModelState.Remove("IsPaid");
            if (ModelState.IsValid)
            {
                _context.Add(fine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BorrowRecordId"] = new SelectList(_context.BorrowRecords, "Id", "Id", fine.BorrowRecordId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", fine.StudentId);
            return View(fine);
        }

        // GET: Fines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fine = await _context.Fines.FindAsync(id);
            if (fine == null) return NotFound();

            ViewData["BorrowRecordId"] = new SelectList(_context.BorrowRecords, "Id", "Id", fine.BorrowRecordId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", fine.StudentId);
            return View(fine);
        }

        // POST: Fines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fine fine)
        {
            if (id != fine.Id) return NotFound();

            ModelState.Remove("Id");
            ModelState.Remove("IsPaid");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Fines.FindAsync(id);
                    if (existing == null) return NotFound();

                    existing.BorrowRecordId = fine.BorrowRecordId;
                    existing.StudentId = fine.StudentId;
                    existing.Amount = fine.Amount;
                    existing.DueDate = fine.DueDate;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineExists(fine.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BorrowRecordId"] = new SelectList(_context.BorrowRecords, "Id", "Id", fine.BorrowRecordId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name", fine.StudentId);
            return View(fine);
        }

        // POST: Fines/MarkPaid/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkPaid(int id)
        {
            var fine = await _context.Fines.FindAsync(id);
            if (fine != null && !fine.IsPaid)
            {
                fine.IsPaid = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Fines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fine = await _context.Fines
                .Include(f => f.BorrowRecord)
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fine == null) return NotFound();

            return View(fine);
        }

        // POST: Fines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fine = await _context.Fines.FindAsync(id);
            if (fine != null)
            {
                _context.Fines.Remove(fine);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FineExists(int id)
        {
            return _context.Fines.Any(e => e.Id == id);
        }
    }
}
