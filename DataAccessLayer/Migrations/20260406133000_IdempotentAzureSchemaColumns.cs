using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <summary>
    /// Idempotent column adds for Azure/prod DBs that drifted behind the EF model (SqlException 207).
    /// Safe no-ops when columns already exist.
    /// </summary>
    public partial class IdempotentAzureSchemaColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Students.FinCode + unique index (root-cause from runtime logs)
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Students', N'FinCode') IS NULL
BEGIN
    ALTER TABLE [dbo].[Students] ADD [FinCode] nvarchar(7) NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_FinCode' AND object_id = OBJECT_ID(N'dbo.Students'))
BEGIN
    CREATE UNIQUE INDEX [IX_Students_FinCode] ON [dbo].[Students] ([FinCode]) WHERE [FinCode] IS NOT NULL;
END
");

            // Students.DocumentSerialNumber + filtered unique index (matches DocumentSerialNumberForRegistration)
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Students', N'DocumentSerialNumber') IS NULL
BEGIN
    ALTER TABLE [dbo].[Students] ADD [DocumentSerialNumber] nvarchar(40) NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_DocumentSerialNumber' AND object_id = OBJECT_ID(N'dbo.Students'))
BEGIN
    CREATE UNIQUE INDEX [IX_Students_DocumentSerialNumber] ON [dbo].[Students] ([DocumentSerialNumber]) WHERE [DocumentSerialNumber] IS NOT NULL;
END
");

            // Teachers.FinCode, TeacherNumber, DocumentSerialNumber + indexes
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Teachers', N'FinCode') IS NULL
BEGIN
    ALTER TABLE [dbo].[Teachers] ADD [FinCode] nvarchar(7) NULL;
END
IF COL_LENGTH(N'dbo.Teachers', N'TeacherNumber') IS NULL
BEGIN
    ALTER TABLE [dbo].[Teachers] ADD [TeacherNumber] nvarchar(32) NULL;
END
IF COL_LENGTH(N'dbo.Teachers', N'DocumentSerialNumber') IS NULL
BEGIN
    ALTER TABLE [dbo].[Teachers] ADD [DocumentSerialNumber] nvarchar(40) NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_FinCode' AND object_id = OBJECT_ID(N'dbo.Teachers'))
BEGIN
    CREATE UNIQUE INDEX [IX_Teachers_FinCode] ON [dbo].[Teachers] ([FinCode]) WHERE [FinCode] IS NOT NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_TeacherNumber' AND object_id = OBJECT_ID(N'dbo.Teachers'))
BEGIN
    CREATE UNIQUE INDEX [IX_Teachers_TeacherNumber] ON [dbo].[Teachers] ([TeacherNumber]) WHERE [TeacherNumber] IS NOT NULL;
END
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_DocumentSerialNumber' AND object_id = OBJECT_ID(N'dbo.Teachers'))
BEGIN
    CREATE UNIQUE INDEX [IX_Teachers_DocumentSerialNumber] ON [dbo].[Teachers] ([DocumentSerialNumber]) WHERE [DocumentSerialNumber] IS NOT NULL;
END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
