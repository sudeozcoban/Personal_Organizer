# 🗂️ Personal Organizer - C# Windows Forms Application

A multi-user **C# Windows Forms application** designed to help users manage personal tasks including notes, reminders, a detailed phone book, personal information, and salary calculation — all while supporting user roles and administrative features.

## 📌 Table of Contents
- [Features](#features)
- [Technologies](#technologies)
- [Modules](#modules)
  - [🔐 User Management](#-user-management)
  - [📞 Phone Book](#-phone-book)
  - [📝 Notes](#-notes)
  - [👤 Personal Information](#-personal-information)
  - [💰 Salary Calculator](#-salary-calculator)
  - [⏰ Reminders](#-reminders)
- [Data Storage](#data-storage)
- [Screenshots](#screenshots)
- [Contributors](#contributors)

---

## 🚀 Features

- Multi-user system with user roles: `admin`, `user`, and `part-time-user`
- CSV, TXT, or XLSX file-based data storage
- Email integration for password resets (with progress bar)
- Profile editing with undo/redo support (`CTRL+Z / CTRL+Y`)
- Modular design (OOP principles, Abstract Factory & Observer patterns)

---

## 🛠 Technologies

- Language: `C#`
- Framework: `.NET Windows Forms`
- Data: `.csv / .txt / .xlsx` files
- UI: Custom Windows Forms GUI
- OOP: Abstract Factory, Observer, Encapsulation, Inheritance, etc.

---

## 📦 Modules

### 🔐 User Management
- First registered user becomes the **Administrator**
- Admin features:
  - Assign user roles (`admin`, `user`, `part-time-user`)
  - Send new passwords via email
- User features:
  - Login / Registration
  - Exit confirmation dialog

### 📞 Phone Book
- Fields: Name, Surname, Phone Number, Address, Email, Description
- CRUD operations: Create, Read, Update, Delete
- Phone format enforced: `(555) 555 55 55`
- Email format validated via **RegEx**

### 📝 Notes
- Single-field notes module
- Supports multiple notes per user
- CRUD operations with data stored in `notes.csv`

### 👤 Personal Information
- View/Edit personal profile:
  - Name, Surname, Phone, Address, Email, Password, Profile Picture
- Change password feature
- Profile picture saved as **Base64** in CSV
- Undo/Redo support (`CTRL+Z / CTRL+Y`)

### 💰 Salary Calculator
- Calculates minimum salary according to **BMO** guidelines:
  - [BMO Rules PDF](https://www.bmo.org.tr/wp-content/uploads/2023/08/BMO_En_Az_Ucret_Tanimlari_R.01_2023-1.pdf)
- Dynamic form: only required fields shown
- Part-time user salaries calculated at **50%** rate
- Displayed on the profile screen

### ⏰ Reminders
- Two types: `Meeting` and `Task` (via Abstract Factory Pattern)
- Fields: Date, Time, Summary, Description
- Reminders:
  - Display summary in window title
  - Shake active window for 2 seconds on trigger (Observer Pattern)

---

## 📂 Data Storage
All user data is stored locally in one of the following formats:
- `.csv` (default)
- `.txt`
- `.xlsx`

Each module uses its own file. For example:
- `users.csv`
- `phonebook.csv`
- `notes.csv`

Phone records are linked to their owner via user IDs.

---
## 📌 Note
Please ensure all data is correctly entered. Invalid formats may result in rejected entries.  
All forms include validation and error handling.
