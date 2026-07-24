using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.MVC.Models
{
    public class Fine
    {
        public int Id { get; set; } // Fine ID

        [Required]
        public int BorrowRecordId { get; set; } // Borrow ID
        public BorrowRecord BorrowRecord { get; set; }

        [Required]
        public int StudentId { get; set; } // Student
        public Student Student { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; } // Paid Status
    }
}
