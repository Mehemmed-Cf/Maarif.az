using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteFilteredUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_Email",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_FinCode",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId_Term_GroupName",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FinCode",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherId_SubjectId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Groups_DepartmentId_Name",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_Name",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_LessonScheduleId_StudentId_SessionDate",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "([DeletedAt] IS NULL) AND ([DocumentSerialNumber] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_FinCode",
                table: "Teachers",
                column: "FinCode",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_TeacherNumber",
                table: "Teachers",
                column: "TeacherNumber",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects",
                columns: new[] { "DepartmentId", "Name" },
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId_Term_GroupName",
                table: "Subjects",
                columns: new[] { "DepartmentId", "Term", "GroupName" },
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "([DeletedAt] IS NULL) AND ([DocumentSerialNumber] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FinCode",
                table: "Students",
                column: "FinCode",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId_SubjectId",
                table: "Lessons",
                columns: new[] { "TeacherId", "SubjectId" },
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DepartmentId_Name",
                table: "Groups",
                columns: new[] { "DepartmentId", "Name" },
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_Name",
                table: "Faculties",
                column: "Name",
                unique: true,
                filter: "[DeletedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LessonScheduleId_StudentId_SessionDate",
                table: "Attendances",
                columns: new[] { "LessonScheduleId", "StudentId", "SessionDate" },
                unique: true,
                filter: "[DeletedAt] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_Email",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_FinCode",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_TeacherNumber",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId_Term_GroupName",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FinCode",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherId_SubjectId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Groups_DepartmentId_Name",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Faculties_Name",
                table: "Faculties");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_LessonScheduleId_StudentId_SessionDate",
                table: "Attendances");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_DocumentSerialNumber",
                table: "Teachers",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "[DocumentSerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects",
                columns: new[] { "DepartmentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId_Term_GroupName",
                table: "Subjects",
                columns: new[] { "DepartmentId", "Term", "GroupName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_DocumentSerialNumber",
                table: "Students",
                column: "DocumentSerialNumber",
                unique: true,
                filter: "[DocumentSerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FinCode",
                table: "Students",
                column: "FinCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId_SubjectId",
                table: "Lessons",
                columns: new[] { "TeacherId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_DepartmentId_Name",
                table: "Groups",
                columns: new[] { "DepartmentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_Name",
                table: "Faculties",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LessonScheduleId_StudentId_SessionDate",
                table: "Attendances",
                columns: new[] { "LessonScheduleId", "StudentId", "SessionDate" },
                unique: true);
        }
    }
}
