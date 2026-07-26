using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.MVC.Data;
using LibraryManagement.MVC.ViewModels;
using LibraryManagement.MVC.Models;

namespace LibraryManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryDbContext _context;
        public HomeController(LibraryDbContext context) { _context = context; }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;

            var allPublications = await _context.Publications.ToListAsync();
            var allBooks = await _context.Books.ToListAsync();
            var allFines = await _context.Fines.ToListAsync();
            var allBorrows = await _context.BorrowRecords
                .Include(b => b.Student)
                .Include(b => b.Book)
                .Include(b => b.Publication)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            var vm = new DashboardViewModel
            {
                TotalBooks = allBooks.Sum(b => b.TotalCopies),
                AvailableBooks = allBooks.Sum(b => b.AvailableCopies),
                BorrowedBooks = allBorrows.Count(b => b.ReturnDate == null),
                TotalStudents = await _context.Students.CountAsync(),
                TotalLibrarians = await _context.Librarians.CountAsync(),
                TotalMagazines = await _context.Magazines.CountAsync(),
                TotalNewspapers = await _context.Newspapers.CountAsync(),
                TotalPublications = allPublications.Count,
                TodaysBorrowings = allBorrows.Count(b => b.BorrowDate.Date == today),
                TodaysReturns = allBorrows.Count(b => b.ReturnDate != null && b.ReturnDate.Value.Date == today),
                RecentBorrows = allBorrows.Take(5).ToList(),
                FeaturedBooks = allBooks.Where(b => b.IsAvailable).OrderByDescending(b => b.Id).Take(3).ToList()
            };

            vm.TotalFine = allFines.Sum(f => f.Amount);
            vm.CollectedFine = allFines.Where(f => f.Status == "Paid").Sum(f => f.Amount);
            vm.PendingFine = vm.TotalFine - vm.CollectedFine;

            // Generate Monthly Borrow Trend (Last 6 Months)
            for (int i = 5; i >= 0; i--)
            {
                var monthDate = today.AddMonths(-i);
                vm.MonthlyLabels.Add(monthDate.ToString("MMM yyyy"));
                vm.MonthlyBorrowCounts.Add(allBorrows.Count(b => b.BorrowDate.Year == monthDate.Year && b.BorrowDate.Month == monthDate.Month));
            }

            // Generate Books Category Doughnut Chart
            var categories = allBooks.GroupBy(b => string.IsNullOrWhiteSpace(b.Category) ? "Uncategorized" : b.Category)
                                     .OrderByDescending(g => g.Count())
                                     .Take(5)
                                     .ToList();
                                     
            foreach (var cat in categories)
            {
                vm.CategoryLabels.Add(cat.Key);
                vm.CategoryCounts.Add(cat.Count());
            }

            return View(vm);
        }
    }
}
