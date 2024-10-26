using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Authors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookID = table.Column<int>(type: "int", nullable: false),
                    BorrowerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Borrowers_BorrowerID",
                        column: x => x.BorrowerID,
                        principalTable: "Borrowers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "ID", "Biography", "BirthDate", "Name", "Nationality" },
                values: new object[,]
                {
                    { -6, "English modernist writer and feminist icon", new DateTime(1882, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Virginia Woolf", "English" },
                    { -5, "American writer known for 'The Catcher in the Rye'", new DateTime(1919, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.D. Salinger", "American" },
                    { -4, "English novelist known for her romantic fiction", new DateTime(1775, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane Austen", "English" },
                    { -3, "English novelist and essayist, known for his dystopian novel '1984'", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "George Orwell", "British" },
                    { -2, "American novelist widely known for 'To Kill a Mockingbird'", new DateTime(1926, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harper Lee", "American" },
                    { -1, "American writer known for his novels and stories", new DateTime(1896, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "F. Scott Fitzgerald", "American" }
                });

            migrationBuilder.InsertData(
                table: "Borrowers",
                columns: new[] { "ID", "Address", "DateOfBirth", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { -6, "999 Walnut St", new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily@example.com", "Emily", "Brown", "7778889999" },
                    { -5, "555 Cedar St", new DateTime(1995, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "michael@example.com", "Michael", "Wilson", "4445556666" },
                    { -4, "321 Pine St", new DateTime(1988, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "sarah@example.com", "Sarah", "Davis", "1112223333" },
                    { -3, "789 Oak St", new DateTime(1985, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "david@example.com", "David", "Johnson", "5555555555" },
                    { -2, "456 Elm St", new DateTime(1992, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane", "Smith", "9876543210" },
                    { -1, "123 Main St", new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John", "Doe", "1234567890" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "ID", "AuthorID", "ISBN", "NumberOfPages", "PublicationDate", "Quantity", "Title" },
                values: new object[,]
                {
                    { -6, -6, "9780156027328", 209, new DateTime(1927, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "To the Lighthouse" },
                    { -5, -5, "9780316769488", 277, new DateTime(1951, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "The Catcher in the Rye" },
                    { -4, -4, "9780141439518", 432, new DateTime(1813, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Pride and Prejudice" },
                    { -3, -3, "9780451524935", 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "1984" },
                    { -2, -2, "9780061120084", 324, new DateTime(1960, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "To Kill a Mockingbird" },
                    { -1, -1, "9780743273565", 218, new DateTime(1925, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "The Great Gatsby" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "ID", "BookID", "BorrowerID", "LoanDate", "ReturnDate" },
                values: new object[,]
                {
                    { -6, -6, -6, new DateTime(2022, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -5, -5, -5, new DateTime(2022, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -4, -4, -4, new DateTime(2022, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -3, -3, -3, new DateTime(2022, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -2, -2, -2, new DateTime(2022, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { -1, -1, -1, new DateTime(2022, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorID",
                table: "Books",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookID",
                table: "Loans",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerID",
                table: "Loans",
                column: "BorrowerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Borrowers");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
