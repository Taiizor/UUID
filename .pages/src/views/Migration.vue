<template>
  <div class="migration">
    <h1>Migration Guide</h1>

    <section id="migrating-from-guid">
      <h2>Migrating from Guid</h2>

      <div class="example-card">
        <h3>Basic Migration</h3>
        <code-block>{{ basicMigration.code }}</code-block>
        <p>The UUID library provides a modern, high-performance alternative to System.Guid with additional features and better performance.</p>
      </div>

      <div class="example-card">
        <h3>Converting Existing Guids</h3>
        <code-block>{{ guidConversion.code }}</code-block>
        <p>UUID provides both explicit conversion methods and implicit operators for seamless integration with existing Guid-based code.</p>
      </div>

      <div class="example-card">
        <h3>Database Migration</h3>
        <code-block>{{ databaseMigration.code }}</code-block>
        <p>The UUID library integrates seamlessly with popular ORMs and data access libraries.</p>
      </div>
    </section>

    <section id="migrating-from-others">
      <h2>Migrating from Other UUID Libraries</h2>

      <div class="example-card">
        <h3>String Format Conversion</h3>
        <code-block>{{ stringFormat.code }}</code-block>
        <p>UUID supports multiple string formats for different use cases, making it easy to migrate from other UUID implementations.</p>
      </div>

      <div class="example-card">
        <h3>Performance Optimization</h3>
        <code-block>{{ performanceOptimization.code }}</code-block>
        <p>UUID provides high-performance methods and modern C# features like Span&lt;T&gt; for optimal performance.</p>
      </div>
    </section>

    <section id="migration-checklist">
      <h2>Migration Checklist</h2>

      <div class="migration-steps">
        <div class="step-card">
          <h3><i class="fas fa-clipboard-check"></i> Before Migration</h3>
          <ul class="checklist">
            <li v-for="(item, index) in beforeMigration" :key="'before-' + index">
              <i class="fas fa-check"></i> {{ item }}
            </li>
          </ul>
        </div>

        <div class="step-card">
          <h3><i class="fas fa-cogs"></i> During Migration</h3>
          <ul class="checklist">
            <li v-for="(item, index) in duringMigration" :key="'during-' + index">
              <i class="fas fa-check"></i> {{ item }}
            </li>
          </ul>
        </div>

        <div class="step-card">
          <h3><i class="fas fa-flag-checkered"></i> After Migration</h3>
          <ul class="checklist">
            <li v-for="(item, index) in afterMigration" :key="'after-' + index">
              <i class="fas fa-check"></i> {{ item }}
            </li>
          </ul>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import Prism from 'prismjs'
import 'prismjs/themes/prism-tomorrow.css'
import 'prismjs/components/prism-csharp'
import CodeBlock from '@/components/CodeBlock.vue'

export default {
  name: 'Migration',
  components: {
    CodeBlock
  },
  data() {
    return {
      basicMigration: {
        code: `// Before (Guid)
Guid guid = Guid.NewGuid();
string guidString = guid.ToString();

// After (UUID)
UUID uuid = new UUID();
string uuidString = uuid.ToString();`
      },
      guidConversion: {
        code: `// Convert from Guid to UUID
Guid existingGuid = Guid.NewGuid();
UUID uuid = UUID.FromGuid(existingGuid);

// Convert back to Guid if needed
Guid convertedBack = uuid.ToGuid();

// Using implicit operators
UUID fromGuid = existingGuid;  // Implicit conversion
Guid toGuid = uuid;           // Implicit conversion`
      },
      databaseMigration: {
        code: `// Entity Framework Configuration
public class MyDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyEntity>()
            .Property(e => e.Id)
            .HasConversion(
                uuid => uuid.ToString(),
                str => UUID.Parse(str)
            );
    }
}

// Dapper Type Handler
public class UUIDTypeHandler : SqlMapper.TypeHandler<UUID>
{
    public override UUID Parse(object value)
    {
        return UUID.Parse((string)value);
    }

    public override void SetValue(IDbDataParameter parameter, UUID value)
    {
        parameter.Value = value.ToString();
    }
}

// Register the Dapper handler
SqlMapper.AddTypeHandler(new UUIDTypeHandler());`
      },
      stringFormat: {
        code: `// Common string formats supported
UUID uuid = new UUID();

// Standard format
string standard = uuid.ToString();  // "0123456789abcdef0123456789abcdef"

// URL-safe format
string urlSafe = uuid.ToBase32();   // "028T5CY4TQKFF028T5CY4TQKFF"

// Base64 format
string base64 = uuid.ToBase64();    // "782riWdFIwHvzauJZ0UjAQ=="`
      },
      performanceOptimization: {
        code: `// Reusable buffers for high-performance scenarios
public class UUIDConverter
{
    private readonly char[] _buffer = new char[32];
    
    public string FastConvert(UUID uuid)
    {
        if (uuid.TryWriteStringify(_buffer))
        {
            return new string(_buffer);
        }
        return uuid.ToString();
    }
}

// Using Span for better performance
public class UUIDByteConverter
{
    public byte[] ToBytes(UUID uuid)
    {
        Span buffer = stackalloc byte[16];
        if (uuid.TryWriteBytes(buffer))
        {
            return buffer.ToArray();
        }
        return uuid.ToByteArray();
    }
}`
      },
      beforeMigration: [
        'Identify all Guid/UUID usages in your codebase',
        'Review string format requirements',
        'Check database schema compatibility',
        'Test performance with your typical workload'
      ],
      duringMigration: [
        'Update model properties from Guid to UUID',
        'Configure ORM mappings',
        'Update API contracts if necessary',
        'Implement conversion logic for external systems'
      ],
      afterMigration: [
        'Verify all UUID operations work as expected',
        'Confirm database operations are successful',
        'Run performance tests',
        'Update documentation'
      ]
    }
  },
  mounted() {
    this.$nextTick(() => {
      Prism.highlightAll()
    })
  }
}
</script>

<style scoped>
.migration {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #fff;
  animation: slideDown 0.8s ease-out;
}

h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #fff;
  border-bottom: 2px solid #ffd700;
  padding-bottom: 0.5rem;
  animation: slideRight 0.8s ease-out;
}

section {
  margin-bottom: 3rem;
  animation: fadeIn 1s ease-out;
}

.example-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: slideUp 0.8s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.example-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}

.step-card {
  animation: slideIn 0.8s ease-out;
  transition: transform 0.3s ease;
}

.step-card:hover {
  transform: translateY(-4px);
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideRight {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.migration-steps {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  margin: 20px 0;
}

.step-card {
  flex: 1;
  min-width: 300px;
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.step-card h3 {
  color: #ffd700;
  margin-bottom: 15px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.checklist {
  list-style: none;
  padding: 0;
  margin: 0;
}

.checklist li {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 10px;
  line-height: 1.4;
  color: #a7a7a7;
}

.checklist li i {
  color: #4caf50;
  font-size: 0.9em;
}

@media (max-width: 768px) {
  .migration {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .example-card h3 {
    font-size: 1.2rem;
  }

  .code-block {
    padding: 15px;
  }

  .step-card {
    min-width: 100%;
  }
}
</style>
