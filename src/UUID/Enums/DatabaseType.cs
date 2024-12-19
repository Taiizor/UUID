namespace System
{
    /// <summary>
    /// Specifies the type of database for UUID optimization.
    /// Different databases handle UUID storage and indexing differently,
    /// so UUID generation can be optimized based on the target database.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// Default database type, uses Version 4 (random) UUIDs.
        /// Suitable when database-specific optimization is not required.
        /// </summary>
        Other = 0,

        /// <summary>
        /// PostgreSQL database.
        /// Uses Version 7 UUIDs with timestamp in the first 48 bits.
        /// This format provides better index performance in PostgreSQL
        /// as it maintains temporal ordering in B-tree indexes.
        /// </summary>
        PostgreSQL = 1,

        /// <summary>
        /// Microsoft SQL Server database.
        /// Uses Version 8 UUIDs with timestamp in the last 48 bits.
        /// This format is optimized for SQL Server's index structure
        /// and helps prevent page splits in clustered indexes.
        /// </summary>
        SQLServer = 2,

        /// <summary>
        /// SQLite database.
        /// Currently treated the same as Other, using Version 4 UUIDs.
        /// SQLite's B-tree implementation doesn't benefit significantly
        /// from timestamp-ordered UUIDs.
        /// </summary>
        SQLite = 3,

        /// <summary>
        /// MySQL/MariaDB database.
        /// Currently treated the same as Other, using Version 4 UUIDs.
        /// Future versions may implement specific optimizations for
        /// MySQL's InnoDB storage engine.
        /// </summary>
        MySQL = 4,

        /// <summary>
        /// Oracle database.
        /// Currently treated the same as Other, using Version 4 UUIDs.
        /// Future versions may implement specific optimizations for
        /// Oracle's index structures.
        /// </summary>
        Oracle = 5
    }
}