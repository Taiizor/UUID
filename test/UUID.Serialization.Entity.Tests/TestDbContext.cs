using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace UUIDSerializationEntityTests
{
    public class TestDbContext : DbContext
    {
        private readonly Action<ModelBuilder>? _onModelCreating;
        private readonly Action<ModelConfigurationBuilder>? _onConfigureConventions;
        private readonly string _cacheKey;

        public TestDbContext(
            DbContextOptions<TestDbContext> options,
            Action<ModelBuilder>? onModelCreating = null,
            Action<ModelConfigurationBuilder>? onConfigureConventions = null,
            string? cacheKey = null)
            : base(options)
        {
            _onModelCreating = onModelCreating;
            _onConfigureConventions = onConfigureConventions;
            // Unique key per instance by default to isolate EF model cache between tests.
            // Without this, EF reuses the first built model for the same DbContext type,
            // so tests configuring different converters (bytes/string/base64) flakily
            // share one model depending on execution order/parallelism.
            _cacheKey = cacheKey ?? Guid.NewGuid().ToString("N");
        }

        public string CacheKey => _cacheKey;

        public static DbContextOptions<TestDbContext> CreateOptions(Microsoft.Data.Sqlite.SqliteConnection connection)
        {
            return new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .ReplaceService<IModelCacheKeyFactory, TestModelCacheKeyFactory>()
                .Options;
        }

        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestEntity>(entity =>
            {
                entity.ToTable("TestEntities");
            });

            _onModelCreating?.Invoke(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            if (_onConfigureConventions is not null)
            {
                _onConfigureConventions.Invoke(configurationBuilder);
            }
            else
            {
                // Default to binary storage
                configurationBuilder.UseUUIDAsBinary();
            }
        }
    }

    public sealed class TestModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context, bool designTime)
        {
            if (context is TestDbContext testContext)
            {
                return (context.GetType(), testContext.CacheKey, designTime);
            }

            return (context.GetType(), designTime);
        }
    }
}