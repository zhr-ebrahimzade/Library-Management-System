# Library Management System

Library Management System is a web-based application built with ASP.NET Core that provides a comprehensive solution for managing books, authors, borrowers, and loans in a library. It offers features for book and borrower management, book borrowing and returning, searching for books and borrowers, and generating reports.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)

## Overview

Library Management System is designed to streamline the management of books and borrowers in a library setting. It simplifies tasks such as adding and updating books, managing borrower information, and keeping track of book loans. With its intuitive interface and powerful features, the system aims to enhance efficiency and improve the overall library experience.

## Features

- **Book Management**: Add, update, and delete books. Search and filter books by title, author, or ISBN.
- **Borrower Management**: Manage borrower information, including name, contact details, and borrowing history.
- **Book Borrowing**: Borrow books for borrowers and keep track of loan details, due dates, and return status.
- **Book Returning**: Record the return of books and update the loan status accordingly.
- **Search and Filtering**: Search for books and borrowers based on various criteria, such as title, author, borrower name, or ISBN.
- **Reports Generation**: Generate reports on book availability, borrower statistics, and loan history.

## Getting Started

To get started with the Library Management System, follow these steps:

1. Clone the repository: `git clone https://github.com/your-username/library-management-system.git`
2. Install the required dependencies: `dotnet restore`
3. Set up the database: Configure the connection string in the `appsettings.json` file and run the migrations using `dotnet ef database update`.
4. Build the application: `dotnet build`
5. Run the application: `dotnet run`
6. Access the application in your web browser at `http://localhost:5000`
   
