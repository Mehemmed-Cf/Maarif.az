using DataAccessLayer.Extensions;
using DataAccessLayer.Migrations;
using Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Tests.DataAccessLayer;

/// <summary>
/// Ensures unique indexes on soft-deleted entity types use <see cref="SoftDeleteIndexExtensions.IsUniqueWhenNotDeleted"/>.
/// </summary>
public sealed class SoftDeleteUniqueIndexConventionTests
{
    private sealed class StubIdentityService : IIdentityService
    {
        public int? GetPrincipialId() => 1;
    }

    [Fact]
    public void Unique_indexes_on_types_with_DeletedAt_use_deleted_at_null_filter()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=__ef_model_only__;Trusted_Connection=True")
            .Options;

        using var context = new DataContext(options, new StubIdentityService());
        var model = context.Model;
        var expected = SoftDeleteIndexExtensions.DeletedAtNullFilterSql;

        foreach (var entityType in model.GetEntityTypes())
        {
            if (entityType.IsOwned())
                continue;

            if (entityType.FindProperty("DeletedAt") is null)
                continue;

            foreach (var index in entityType.GetIndexes())
            {
                if (!index.IsUnique)
                    continue;

                var filter = index.GetFilter();
                Assert.True(
                    filter is not null
                    && filter.Contains(expected, StringComparison.OrdinalIgnoreCase),
                    $"Entity '{entityType.ClrType.Name}' unique index '{index.GetDatabaseName()}' must include soft-delete filter \"{expected}\" (soft-delete convention). Actual: '{filter ?? "(null)"}'.");
            }
        }
    }
}
