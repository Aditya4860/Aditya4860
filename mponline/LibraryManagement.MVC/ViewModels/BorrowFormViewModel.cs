using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagement.MVC.ViewModels
{
    public class BorrowFormViewModel
    {
        [Required(ErrorMessage = "Please select a student.")]
        public int StudentId { get; set; }
        
        public int? BookId { get; set; }
        
        public int? PublicationId { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; } = DateTime.Today;
        [Required(ErrorMessage = "Please select an issuing librarian.")]
        public int LibrarianId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(15);
        
        // These are populated by the controller for the dropdowns
        public SelectList StudentsList { get; set; }
        public SelectList AvailableBooksList { get; set; }
        public SelectList AvailablePublicationsList { get; set; }
        public SelectList LibrariansList { get; set; }
    }
}
