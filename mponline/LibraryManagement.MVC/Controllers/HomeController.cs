using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.MVC.Data;
using LibraryManagement.MVC.ViewModels;
using LibraryManagement.MVC.Models;

namespace LibraryManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryDbContext _context;
        public HomeController(LibraryDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;

            var allPublications = await _context.Publications.ToListAsync();
            var allBooks = await _context.Books.ToListAsync();
            var allFines = await _context.Fines.ToListAsync();
            var allBorrows = await _context.BorrowRecords.ToListAsync();

            var vm = new DashboardViewModel
            {
                TotalBooks = allBooks.Count,
                AvailableBooks = allBooks.Count(b => b.IsAvailable),
                BorrowedBooks = allBooks.Count(b => !b.IsAvailable),
                TotalStudents = await _context.Students.CountAsync(),
                TotalLibrarians = await _context.Librarians.CountAsync(),
                TotalMagazines = allPublications.Count(p => p.Type == PublicationType.Magazine),
                TotalNewspapers = allPublications.Count(p => p.Type == PublicationType.Newspaper),
                TotalPublications = allPublications.Count,
                TodaysBorrowings = allBorrows.Count(b => b.BorrowDate.Date == today),
                TodaysReturns = allBorrows.Count(b => b.ReturnDate != null && b.ReturnDate.Value.Date == today)
            };

            vm.TotalFine = allFines.Sum(f => f.Amount);
            vm.CollectedFine = allFines.Where(f => f.IsPaid).Sum(f => f.Amount);
            vm.PendingFine = vm.TotalFine - vm.CollectedFine;

            return View(vm);
        }
    }
}
