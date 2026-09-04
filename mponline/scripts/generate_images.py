import os
from PIL import Image, ImageDraw, ImageFont
from docx import Document
from docx.shared import Inches, Pt
from docx.enum.text import WD_ALIGN_PARAGRAPH
import shutil

# Ensure directories exist
os.makedirs("Generated_Diagrams", exist_ok=True)
os.makedirs("Generated_Screenshots", exist_ok=True)

def create_placeholder_image(filename, text, size=(800, 600), bg_color=(240, 240, 240)):
    if not os.path.exists(filename):
        img = Image.new('RGB', size, color=bg_color)
        d = ImageDraw.Draw(img)
        # We don't have standard fonts, rely on default font
        try:
            font = ImageFont.truetype("arial.ttf", 36)
        except IOError:
            font = ImageFont.load_default()
            
        # Quick center text manually (for default font which doesn't support getsize easily in newer Pillow)
        d.text((size[0]//2 - 100, size[1]//2), text, fill=(0,0,0), font=font)
        img.save(filename)
        print(f"Generated placeholder for {filename}")

# Generate Diagram Placeholders
diagrams = ["ER Diagram", "Use Case Diagram", "Activity Diagram", "Sequence Diagram", 
            "Class Diagram", "System Architecture", "DFD Level 0", "DFD Level 1", 
            "Database Relationship Diagram"]

for d in diagrams:
    file_path = f"Generated_Diagrams/{d.replace(' ', '_').lower()}.png"
    create_placeholder_image(file_path, d, size=(800, 600))

# Generate missing Screenshots
existing_screenshots_dir = "docs/screenshots"
required_screenshots = ["Login", "Register", "Forgot Password", "Home", "Dashboard", 
                        "Books List", "Search Books", "Pagination", "Add Book", 
                        "Edit Book", "Delete Book", "Book Details", "Borrow Book", 
                        "Return Book", "Student List", "Student Search", "Student Pagination", 
                        "Add Student", "Edit Student", "Delete Student", "Librarian List", 
                        "Librarian Search", "Librarian Pagination", "Add Librarian", 
                        "Edit Librarian", "Delete Librarian", "Magazine Module", 
                        "Newspaper Module", "About Us", "Contact Us", "Identity Profile", 
                        "Change Password", "Database", "SQL Tables", "Unit Testing", 
                        "Test Explorer", "Passed Tests", "Responsive Layout", 
                        "Navigation Bar", "Footer"]

# Check which are mapped to existing docs/screenshots
mapped = {
    "Login": "login.png",
    "Register": "register.png",
    "Home": "landing-page.png",
    "Dashboard": "dashboard.png",
    "Books List": "books.png",
    "Search Books": "search.png",
    "Borrow Book": "borrow.png",
    "Student List": "students.png",
    "Librarian List": "librarians.png",
    "Magazine Module": "magazines.png",
    "Newspaper Module": "newspapers.png"
}

for req in required_screenshots:
    file_name = f"Generated_Screenshots/{req.replace(' ', '_').lower()}.png"
    if req in mapped and os.path.exists(os.path.join(existing_screenshots_dir, mapped[req])):
        shutil.copy(os.path.join(existing_screenshots_dir, mapped[req]), file_name)
    else:
        create_placeholder_image(file_name, req, size=(1024, 768), bg_color=(200, 220, 240))

print("Diagrams and screenshots successfully mapped and generated.")
