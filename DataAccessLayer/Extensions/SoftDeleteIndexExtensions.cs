using Domain.Models.Concrates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Extensions
{
    /// <summary>
    /// Helpers for indexes on entities that use <see cref="AuditableEntity.DeletedAt"/> for soft delete.
    /// </summary>
    /// <remarks>
    /// Convention: any <b>unique</b> index on an entity type that maps <c>DeletedAt</c> must use
    /// <see cref="IsUniqueWhenNotDeleted"/> (or the same filter manually). A plain unique index applies to
    /// all rows, including soft-deleted ones, and conflicts with the global query filter pattern.
    /// </remarks>
    public static class SoftDeleteIndexExtensions
    {
        /// <summary>
        /// SQL Server filter for "row is not soft-deleted". Keep in sync with unique-index tests.
        /// </summary>
        public const string DeletedAtNullFilterSql = "[DeletedAt] IS NULL";

        /// <summary>
        /// Declares a unique index scoped to non-deleted rows only (<c>DeletedAt IS NULL</c>).
        /// </summary>
        public static IndexBuilder IsUniqueWhenNotDeleted(this IndexBuilder builder) =>
            builder.IsUnique().HasFilter(DeletedAtNullFilterSql);

        /// <summary>
        /// Same as <see cref="IsUniqueWhenNotDeleted(IndexBuilder)"/> plus an extra SQL predicate combined with AND
        /// (for example <c>[DocumentSerialNumber] IS NOT NULL</c> on a nullable column).
        /// </summary>
        /// <param name="additionalSqlPredicate">SQL expression without a leading AND.</param>
        public static IndexBuilder IsUniqueWhenNotDeleted(this IndexBuilder builder, string additionalSqlPredicate)
        {
            var extra = additionalSqlPredicate.Trim();
            return builder.IsUnique().HasFilter($"({DeletedAtNullFilterSql}) AND ({extra})");
        }
    }
}
