using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonGroup_Groups_GroupId",
                table: "LessonGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonGroup_Lessons_LessonId",
                table: "LessonGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Groups_GroupId",
                schema: "dbo",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Faculties_FacultyId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherDepartments_Departments_DepartmentId",
                schema: "dbo",
                table: "TeacherDepartments");

            migrationBuilder.DropIndex(
                name: "IX_Students_FacultyId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_TeacherId",
                schema: "dbo",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonGroup",
                table: "LessonGroup");

            migrationBuilder.DropColumn(
                name: "ActiveLessons",
                schema: "dbo",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "GroupCount",
                schema: "dbo",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "FacultyId",
                schema: "dbo",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Teachers",
                schema: "dbo",
                newName: "Teachers");

            migrationBuilder.RenameTable(
                name: "TeacherDepartments",
                schema: "dbo",
                newName: "TeacherDepartments");

            migrationBuilder.RenameTable(
                name: "Subjects",
                schema: "dbo",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "Students",
                schema: "dbo",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "StudentGroups",
                schema: "dbo",
                newName: "StudentGroups");

            migrationBuilder.RenameTable(
                name: "Lessons",
                schema: "dbo",
                newName: "Lessons");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "dbo",
                newName: "Groups");

            migrationBuilder.RenameTable(
                name: "Faculties",
                schema: "dbo",
                newName: "Faculties");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "dbo",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "LessonGroup",
                newName: "LessonGroups");

            migrationBuilder.RenameIndex(
                name: "IX_LessonGroup_GroupId",
                table: "LessonGroups",
                newName: "IX_LessonGroups_GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Teachers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Teachers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "Experience",
                table: "Teachers",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Students",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Students",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Students",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "EducationType",
                table: "Students",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonGroups",
                table: "LessonGroups",
                columns: new[] { "LessonId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Email",
                table: "Teachers",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects",
                columns: new[] { "DepartmentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

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
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonGroups_Groups_GroupId",
                table: "LessonGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonGroups_Lessons_LessonId",
                table: "LessonGroups",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Groups_GroupId",
                table: "StudentGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherDepartments_Departments_DepartmentId",
                table: "TeacherDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonGroups_Groups_GroupId",
                table: "LessonGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonGroups_Lessons_LessonId",
                table: "LessonGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Groups_GroupId",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherDepartments_Departments_DepartmentId",
                table: "TeacherDepartments");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_Email",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_DepartmentId_Name",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentNumber",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UserId",
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
                name: "IX_Departments_Name",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonGroups",
                table: "LessonGroups");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Teachers",
                newName: "Teachers",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "TeacherDepartments",
                newName: "TeacherDepartments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subjects",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Students",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "StudentGroups",
                newName: "StudentGroups",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "Lessons",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Groups",
                newName: "Groups",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Faculties",
                newName: "Faculties",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Departments",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "LessonGroups",
                newName: "LessonGroup");

            migrationBuilder.RenameIndex(
                name: "IX_LessonGroups_GroupId",
                table: "LessonGroup",
                newName: "IX_LessonGroup_GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                schema: "dbo",
                table: "Teachers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "Teachers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<double>(
                name: "Experience",
                schema: "dbo",
                table: "Teachers",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Teachers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveLessons",
                schema: "dbo",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupCount",
                schema: "dbo",
                table: "Teachers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                schema: "dbo",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<byte>(
                name: "Status",
                schema: "dbo",
                table: "Students",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                schema: "dbo",
                table: "Students",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<byte>(
                name: "Grade",
                schema: "dbo",
                table: "Students",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<byte>(
                name: "Gender",
                schema: "dbo",
                table: "Students",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "FatherName",
                schema: "dbo",
                table: "Students",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<byte>(
                name: "EducationType",
                schema: "dbo",
                table: "Students",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<int>(
                name: "FacultyId",
                schema: "dbo",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Groups",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonGroup",
                table: "LessonGroup",
                columns: new[] { "LessonId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyId",
                schema: "dbo",
                table: "Students",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId",
                schema: "dbo",
                table: "Lessons",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonGroup_Groups_GroupId",
                table: "LessonGroup",
                column: "GroupId",
                principalSchema: "dbo",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonGroup_Lessons_LessonId",
                table: "LessonGroup",
                column: "LessonId",
                principalSchema: "dbo",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Subjects_SubjectId",
                schema: "dbo",
                table: "Lessons",
                column: "SubjectId",
                principalSchema: "dbo",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                schema: "dbo",
                table: "Lessons",
                column: "TeacherId",
                principalSchema: "dbo",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Groups_GroupId",
                schema: "dbo",
                table: "StudentGroups",
                column: "GroupId",
                principalSchema: "dbo",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Faculties_FacultyId",
                schema: "dbo",
                table: "Students",
                column: "FacultyId",
                principalSchema: "dbo",
                principalTable: "Faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherDepartments_Departments_DepartmentId",
                schema: "dbo",
                table: "TeacherDepartments",
                column: "DepartmentId",
                principalSchema: "dbo",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
