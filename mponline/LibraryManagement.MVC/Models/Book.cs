using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.MVC.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Category { get; set; }

        [Range(0, int.MaxValue)]
        public int TotalCopies { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }

        public bool IsAvailable { get; set; } = true;
        
        public string Status => IsAvailable ? "Available" : "Checked Out";
    }
}
