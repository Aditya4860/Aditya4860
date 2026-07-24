using System.Collections.Generic;
using LibraryManagement.MVC.Models;

namespace LibraryManagement.MVC.ViewModels
{
    public class StudentIndexViewModel
    {
        public IEnumerable<Student> Students { get; set; }

        // Pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;

        // Search parameters
        public string SearchName { get; set; }
        public string SearchEmail { get; set; }
        public string SearchPhone { get; set; }
    }
}
