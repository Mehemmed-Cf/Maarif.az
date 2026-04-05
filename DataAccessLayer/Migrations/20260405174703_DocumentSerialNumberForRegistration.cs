using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class DocumentSerialNumberForRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentSerialNumber",
                table: "Teachers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentSerialNumber",
                table: "Students",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "[DocumentSerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "[DocumentSerialNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DocumentSerialNumber",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DocumentSerialNumber",
                table: "Students");
        }
    }
}
