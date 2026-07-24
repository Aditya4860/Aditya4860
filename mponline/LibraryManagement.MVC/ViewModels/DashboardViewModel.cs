using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.MVC.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BorrowedBooks { get; set; }
        
        public int TotalStudents { get; set; }
        public int TotalLibrarians { get; set; }
        public int TotalMagazines { get; set; }
        public int TotalNewspapers { get; set; }
        public int TotalPublications { get; set; }

        public int TodaysBorrowings { get; set; }
        public int TodaysReturns { get; set; }
        
        public decimal TotalFine { get; set; }
        public decimal PendingFine { get; set; }
        public decimal CollectedFine { get; set; }
    }
}
