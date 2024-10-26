using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddActivePropToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -6,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -5,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -4,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -3,
                column: "Active",
                value: false);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -2,
                column: "Active",
                value: true);

            migrationBuilder.UpdateData(
                table: "Loans",
                keyColumn: "ID",
                keyValue: -1,
                column: "Active",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Loans");
        }
    }
}
