using System;
using System.Linq;
using LibraryManagement.MVC.Data;
using LibraryManagement.MVC.Models;

namespace LibraryManagement.MVC
{
    public static class DbSeeder
    {
        public static void Seed(LibraryDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Books.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    context.Books.Add(new Book 
                    { 
                        Title = $"Sample Book {i}", 
                        Author = $"Author {i}", 
                        ISBN = $"978-0-00-0000{i:00}-0", 
                        Category = "Fiction", 
                        TotalCopies = 5, 
                        AvailableCopies = 5, 
                        IsAvailable = true 
                    });
                }
            }

            if (!context.Students.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Students.Add(new Student 
                    { 
                        EnrollmentNo = $"ENR{i:000}", 
                        Name = $"Student {i}", 
                        Email = $"student{i}@example.com", 
                        Phone = "1234567890", 
                        Department = "Computer Science", 
                        Semester = (i % 8) + 1 
                    });
                }
            }

            if (!context.Librarians.Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    context.Librarians.Add(new Librarian 
                    { 
                        Name = $"Librarian {i}", 
                        EmployeeId = $"LIB{i:000}", 
                        Email = $"lib{i}@example.com", 
                        Phone = "1231231234", 
                        Shift = i % 2 == 0 ? "Evening" : "Morning" 
                    });
                }
            }

            if (!context.Publications.Any())
            {
                for (int i = 1; i <= 10; i++)
                {
                    context.Publications.Add(new Publication 
                    { 
                        Title = $"Newspaper {i}", 
                        Publisher = "News Co", 
                        PublishedDate = DateTime.Today.AddDays(-i), 
                        Type = PublicationType.Newspaper, 
                        IsAvailable = true 
                    });
                }
                for (int i = 1; i <= 10; i++)
                {
                    context.Publications.Add(new Publication 
                    { 
                        Title = $"Magazine {i}", 
                        Publisher = "Mag Co", 
                        PublishedDate = DateTime.Today.AddMonths(-i), 
                        Type = PublicationType.Magazine, 
                        IsAvailable = true 
                    });
                }
            }

            context.SaveChanges();

            if (!context.BorrowRecords.Any())
            {
                var students = context.Students.ToList();
                var books = context.Books.ToList();
                var librarians = context.Librarians.ToList();

                for (int i = 0; i < 10; i++)
                {
                    var book = books[i];
                    book.AvailableCopies--;
                    book.IsAvailable = book.AvailableCopies > 0;

                    var isReturned = i % 2 == 0;
                    var isOverdue = i % 3 == 0 && !isReturned;

                    var borrowDate = DateTime.Today.AddDays(-20 + i);
                    var dueDate = borrowDate.AddDays(15);
                    
                    var record = new BorrowRecord
                    {
                        StudentId = students[i].Id,
                        BookId = book.Id,
                        BorrowDate = borrowDate,
                        DueDate = dueDate,
                        IssuedByLibrarianId = librarians[i % librarians.Count].Id,
                        ReturnDate = isReturned ? (DateTime?)borrowDate.AddDays(10) : null,
                        ReturnedByLibrarianId = isReturned ? librarians[(i + 1) % librarians.Count].Id : null
                    };

                    context.BorrowRecords.Add(record);

                    if (isOverdue)
                    {
                        // Needs a fine simulated if it was returned overdue
                    }
                    else if (isReturned && record.ReturnDate > dueDate)
                    {
                        var diffDays = (record.ReturnDate.Value - dueDate).TotalDays;
                        context.Fines.Add(new Fine
                        {
                            BorrowRecordId = record.Id,
                            StudentId = record.StudentId,
                            Amount = (decimal)(diffDays * 10),
                            DueDate = record.ReturnDate.Value.AddDays(7),
                            IsPaid = false
                        });
                    }
                }
                
                // Add a forced pending fine for the dashboard
                var overdueBook = books[15];
                var forcedRecord = new BorrowRecord
                {
                    StudentId = students[0].Id,
                    BookId = overdueBook.Id,
                    BorrowDate = DateTime.Today.AddDays(-30),
                    DueDate = DateTime.Today.AddDays(-15),
                    IssuedByLibrarianId = librarians[0].Id
                };
                context.BorrowRecords.Add(forcedRecord);
                context.SaveChanges();

                // Assign fine for forcedRecord
                context.Fines.Add(new Fine
                {
                    BorrowRecordId = forcedRecord.Id,
                    StudentId = forcedRecord.StudentId,
                    Amount = 150m,
                    DueDate = DateTime.Today.AddDays(-8),
                    IsPaid = false
                });

                context.SaveChanges();
            }
        }
    }
}
