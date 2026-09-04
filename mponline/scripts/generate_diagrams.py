import os
import subprocess

diagrams = {
    "er_diagram.mmd": """erDiagram
    STUDENT ||--o{ BORROW : makes
    BOOK ||--o{ BORROW : is_borrowed
    LIBRARIAN ||--o{ BOOK : manages
    STUDENT {
        int StudentId
        string Name
        string Email
        string Phone
    }
    BOOK {
        int BookId
        string Title
        string Author
        string ISBN
        int Copies
    }
    BORROW {
        int BorrowId
        date BorrowDate
        date ReturnDate
        string Status
    }
    LIBRARIAN {
        int LibrarianId
        string Name
        string Email
    }""",
    
    "use_case.mmd": """usecaseDiagram
    actor Student
    actor Librarian
    actor Admin
    
    Student --> (Search Books)
    Student --> (View Borrow History)
    
    Librarian --> (Manage Books)
    Librarian --> (Issue Book)
    Librarian --> (Accept Return)
    Librarian --> (Manage Students)
    
    Admin --> (Manage Librarians)
    Admin --> (System Settings)""",
    
    "activity.mmd": """stateDiagram-v2
    [*] --> Login
    Login --> Dashboard : Valid Credentials
    Login --> Login : Invalid Credentials
    Dashboard --> SearchBooks
    SearchBooks --> BorrowBook
    BorrowBook --> UpdateInventory
    UpdateInventory --> [*]""",

    "sequence.mmd": """sequenceDiagram
    actor Student
    participant UI as System Interface
    participant DB as Database
    
    Student->>UI: Request to Borrow Book
    UI->>DB: Check Availability
    alt is Available
        DB-->>UI: Return Success
        UI->>DB: Create Borrow Record
        UI-->>Student: Borrow Confirmed
    else not Available
        DB-->>UI: Return Out of Stock
        UI-->>Student: Show Error Message
    end""",

    "class.mmd": """classDiagram
    class Book {
      +int Id
      +string Title
      +string Author
      +borrow()
      +return()
    }
    class User {
      +int Id
      +string Email
      +string Role
      +login()
    }
    class BorrowRecord {
      +int RecordId
      +DateTime IssueDate
      +DateTime DueDate
      +calculateFine()
    }
    User "1" -- "*" BorrowRecord
    Book "1" -- "*" BorrowRecord""",

    "architecture.mmd": """graph TD
    UI[User Interface - ASP.NET Core MVC]
    Controller[Controllers - Business Logic]
    EF[Entity Framework Core - Data Access]
    SQL[(SQL Server Database)]
    
    UI -->|HTTP Requests| Controller
    Controller -->|LINQ Queries| EF
    EF -->|SQL Queries| SQL""",

    "dfd_level_0.mmd": """graph TD
    User([User]) -->|Inputs Data| LMS[Library Management System]
    LMS -->|Displays Info| User""",

    "dfd_level_1.mmd": """graph TD
    User([User]) -->|Credentials| Auth[Authentication Process]
    Auth -->|Validation Result| User
    Librarian([Librarian]) -->|Book Details| Manage[Book Management]
    Manage -->|DB Updates| DB[(Database)]
    Student([Student]) -->|Borrow Request| Borrow[Borrow Process]
    Borrow -->|Status Update| DB"""
}

# The usecase diagram in mermaid is experimental/not fully supported in old versions,
# but we can simulate it with a graph. Let's fix use_case to use standard graph.
diagrams["use_case.mmd"] = """graph LR
    S([Student])
    L([Librarian])
    A([Admin])
    
    S --> SB(Search Books)
    S --> VH(View Borrow History)
    
    L --> MB(Manage Books)
    L --> IB(Issue Book)
    L --> AR(Accept Return)
    L --> MS(Manage Students)
    
    A --> ML(Manage Librarians)
    A --> SS(System Settings)"""

for name, content in diagrams.items():
    with open(name, 'w') as f:
        f.write(content)
    # Generate image
    out_name = f"Generated_Diagrams/{name.replace('.mmd', '.png')}"
    cmd = f"mmdc -i {name} -o {out_name} -t default -b white"
    print(f"Running: {cmd}")
    os.system(cmd)
