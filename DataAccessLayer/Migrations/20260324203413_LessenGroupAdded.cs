using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class LessenGroupAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_GroupId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "GroupId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "LessonGroup",
                columns: table => new
                {
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonGroup", x => new { x.LessonId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_LessonGroup_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "dbo",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonGroup_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "dbo",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonGroup_GroupId",
                table: "LessonGroup",
                column: "GroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonGroup");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                schema: "dbo",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_GroupId",
                schema: "dbo",
                table: "Lessons",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                schema: "dbo",
                table: "Lessons",
                column: "GroupId",
                principalSchema: "dbo",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
