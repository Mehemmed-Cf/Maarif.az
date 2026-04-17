using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <summary>
    /// Idempotent column adds for Azure/prod DBs that drifted behind the EF model (SqlException 207).
    /// Each ALTER and its CREATE INDEX run in separate Sql() calls — SQL Server parses a whole batch at once,
    /// so CREATE INDEX cannot appear in the same batch as ALTER TABLE ADD for that column.
    /// </summary>
    /// <remarks>
    /// Each <see cref="MigrationBuilder.Sql(string)"/> runs as its own batch. Combining
    /// <c>ALTER TABLE ... ADD</c> with <c>CREATE INDEX (... [NewCol])</c> in one batch can cause
    /// error 207 (invalid column name) because SQL Server may resolve the index before the alter runs.
    /// </remarks>
    public partial class IdempotentAzureSchemaColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
<<<<<<< HEAD
=======
            // Students.FinCode — column first, index in a separate batch
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Students', N'FinCode') IS NULL
    ALTER TABLE [dbo].[Students] ADD [FinCode] nvarchar(7) NULL;
<<<<<<< HEAD
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_FinCode' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE UNIQUE INDEX [IX_Students_FinCode] ON [dbo].[Students] ([FinCode]) WHERE [FinCode] IS NOT NULL;
");

=======
END
");
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_FinCode' AND object_id = OBJECT_ID(N'dbo.Students'))
BEGIN
    CREATE UNIQUE INDEX [IX_Students_FinCode] ON [dbo].[Students] ([FinCode]) WHERE [FinCode] IS NOT NULL;
END
");

            // Students.DocumentSerialNumber
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Students', N'DocumentSerialNumber') IS NULL
    ALTER TABLE [dbo].[Students] ADD [DocumentSerialNumber] nvarchar(40) NULL;
<<<<<<< HEAD
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_DocumentSerialNumber' AND object_id = OBJECT_ID(N'dbo.Students'))
    CREATE UNIQUE INDEX [IX_Students_DocumentSerialNumber] ON [dbo].[Students] ([DocumentSerialNumber]) WHERE [DocumentSerialNumber] IS NOT NULL;
");

=======
END
");
            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Students_DocumentSerialNumber' AND object_id = OBJECT_ID(N'dbo.Students'))
BEGIN
    CREATE UNIQUE INDEX [IX_Students_DocumentSerialNumber] ON [dbo].[Students] ([DocumentSerialNumber]) WHERE [DocumentSerialNumber] IS NOT NULL;
END
");

            // Teachers: add all new columns in one batch (no CREATE INDEX here)
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
            migrationBuilder.Sql(@"
IF COL_LENGTH(N'dbo.Teachers', N'FinCode') IS NULL
    ALTER TABLE [dbo].[Teachers] ADD [FinCode] nvarchar(7) NULL;
IF COL_LENGTH(N'dbo.Teachers', N'TeacherNumber') IS NULL
    ALTER TABLE [dbo].[Teachers] ADD [TeacherNumber] nvarchar(32) NULL;
IF COL_LENGTH(N'dbo.Teachers', N'DocumentSerialNumber') IS NULL
    ALTER TABLE [dbo].[Teachers] ADD [DocumentSerialNumber] nvarchar(40) NULL;
<<<<<<< HEAD
=======
END
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_FinCode' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE UNIQUE INDEX [IX_Teachers_FinCode] ON [dbo].[Teachers] ([FinCode]) WHERE [FinCode] IS NOT NULL;
<<<<<<< HEAD
=======
END
");
            migrationBuilder.Sql(@"
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_TeacherNumber' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE UNIQUE INDEX [IX_Teachers_TeacherNumber] ON [dbo].[Teachers] ([TeacherNumber]) WHERE [TeacherNumber] IS NOT NULL;
<<<<<<< HEAD
=======
END
");
            migrationBuilder.Sql(@"
>>>>>>> d494ec92 (Added Attendance and detached docker and Superadmin role for faster development)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Teachers_DocumentSerialNumber' AND object_id = OBJECT_ID(N'dbo.Teachers'))
    CREATE UNIQUE INDEX [IX_Teachers_DocumentSerialNumber] ON [dbo].[Teachers] ([DocumentSerialNumber]) WHERE [DocumentSerialNumber] IS NOT NULL;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
