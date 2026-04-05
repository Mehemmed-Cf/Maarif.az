using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class TeacherRegistrationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinCode",
                table: "Teachers",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherNumber",
                table: "Teachers",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_FinCode",
                table: "Teachers",
                column: "FinCode",
                unique: true,
                filter: "[FinCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherNumber",
                table: "Teachers",
                column: "TeacherNumber",
                unique: true,
                filter: "[TeacherNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_FinCode",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherNumber",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FinCode",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "TeacherNumber",
                table: "Teachers");
        }
    }
}
