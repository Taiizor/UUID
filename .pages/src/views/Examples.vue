<template>
  <div class="examples">
    <h1>Examples</h1>

    <section id="basic-examples">
      <h2>Basic Examples</h2>

      <div class="example-card" v-for="example in basicExamples" :key="example.id">
        <h3>{{ example.title }}</h3>
        <p>{{ example.description }}</p>
        <code-block>{{ example.code }}</code-block>
      </div>
    </section>

    <section id="advanced-examples">
      <h2>Advanced Examples</h2>

      <div class="example-card" v-for="example in advancedExamples" :key="example.id">
        <h3>{{ example.title }}</h3>
        <p>{{ example.description }}</p>
        <code-block>{{ example.code }}</code-block>
      </div>
    </section>

    <section id="string-format-examples">
      <h2>String Format Examples</h2>

      <div class="example-card" v-for="example in stringFormatExamples" :key="example.id">
        <h3>{{ example.title }}</h3>
        <p>{{ example.description }}</p>
        <code-block>{{ example.code }}</code-block>
      </div>
    </section>

    <section id="database-examples">
      <h2>Database Examples</h2>

      <div class="example-card" v-for="example in databaseExamples" :key="example.id">
        <h3>{{ example.title }}</h3>
        <p>{{ example.description }}</p>
        <code-block>{{ example.code }}</code-block>
      </div>
    </section>

    <section id="web-api-examples">
      <h2>Web API Examples</h2>

      <div class="example-card" v-for="example in webApiExamples" :key="example.id">
        <h3>{{ example.title }}</h3>
        <p>{{ example.description }}</p>
        <code-block>{{ example.code }}</code-block>
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
  name: 'Examples',
  components: {
    CodeBlock
  },
  data() {
    return {
      basicExamples: [
        {
          id: 1,
          title: 'Generate and Format UUIDs',
          code: `// Generate a new UUID
var id = new UUID();

// Different string formats
Console.WriteLine(id.ToString()); // Standard format
Console.WriteLine(id.ToBase64()); // Base64 format
Console.WriteLine(id.ToBase32()); // Base32 format
Console.WriteLine(id.Int64()); // Int64 format

// Get raw bytes
byte[] bytes = id.ToByteArray();`,
          description: 'This example shows different ways to create and format UUIDs.'
        },
        {
          id: 2,
          title: 'Parse UUIDs',
          code: `// Parse from string
var id1 = UUID.Parse("0123456789abcdef0123456789abcdef");

// Try parse pattern
if (UUID.TryParse("0123456789abcdef0123456789abcdef", out var id2))
{
    Console.WriteLine("Successfully parsed: " + id2);
}`,
          description: 'Examples of parsing UUIDs from different string formats.'
        }
      ],

      advancedExamples: [
        {
          id: 1,
          title: 'Bulk UUID Generation',
          code: `// Generate multiple UUIDs efficiently
UUID[] uuids = new UUID[1000];
ArrayExtension.Fill(uuids);

// Process in parallel
Parallel.ForEach(uuids, uuid =>
{
    ProcessUUID(uuid);
});

private void ProcessUUID(UUID uuid)
{
    // Your processing logic here
    var str = uuid.ToString();
    // ...
}`,
          description: 'Example of generating and processing multiple UUIDs efficiently.'
        },
        {
          id: 2,
          title: 'Custom Formatting',
          code: `var uuid = new UUID();

// Custom string builder formatting
var sb = new StringBuilder();
sb.Append("UUID-");
sb.Append(uuid.ToString());
sb.Append("-END");

// Format with prefix
string prefixed = $"ID_{uuid}";`,
          description: 'Examples of custom UUID string formatting.'
        }
      ],

      stringFormatExamples: [
        {
          id: 1,
          title: 'String Format Conversions',
          code: `// Create a new UUID
var id = new UUID();

// Convert to different formats
string standard = id.ToString();
string base64 = id.ToBase64();
string base32 = id.ToBase32();
long int64 = id.ToInt64();

Console.WriteLine($"Standard: {standard}");
Console.WriteLine($"Base64: {base64}");
Console.WriteLine($"Base32: {base32}");
Console.WriteLine($"Int64: {int64}");

// Convert back from Base64
UUID fromBase64 = UUID.FromBase64(base64);
Console.WriteLine($"Restored from Base64: {fromBase64}");

// Safe parsing from Base64
if (UUID.TryFromBase64(base64, out UUID parsed))
{
    Console.WriteLine($"Successfully parsed from Base64: {parsed}");
}`,
          description: 'Examples of converting UUIDs to and from different string formats.'
        },
        {
          id: 2,
          title: 'Byte Array Operations',
          code: `// Create a new UUID
var id = new UUID();

// Convert to byte array
byte[] bytes = id.ToByteArray();
Console.WriteLine($"As bytes: {BitConverter.ToString(bytes)}");

// Convert back from bytes
UUID fromBytes = UUID.FromByteArray(bytes);
Console.WriteLine($"Restored from bytes: {fromBytes}");

// Safe conversion with TryFromByteArray
if (UUID.TryFromByteArray(bytes, out UUID parsed))
{
    Console.WriteLine($"Successfully parsed from bytes: {parsed}");
}

// Write directly to a byte array
byte[] destination = new byte[16];
if (id.TryWriteBytes(destination))
{
    Console.WriteLine($"Successfully wrote to byte array: {BitConverter.ToString(destination)}");
}`,
          description: 'Examples of working with byte array representations of UUIDs.'
        },
        {
          id: 3,
          title: 'Guid Conversions',
          code: `// Create a new UUID
var uuid = new UUID();

// Implicit conversion to Guid
Guid guid = uuid;
Console.WriteLine($"As Guid: {guid}");

// Implicit conversion from Guid
UUID fromGuid = guid;
Console.WriteLine($"Back to UUID: {fromGuid}");

// Using explicit methods
Guid guidExplicit = uuid.ToGuid();
UUID uuidExplicit = UUID.FromGuid(guidExplicit);`,
          description: 'Examples of converting between UUID and Guid types.'
        }
      ],

      databaseExamples: [
        {
          id: 1,
          title: 'Entity Framework Core',
          code: `public class User
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString(),
                v => UUID.Parse(v));
    }
}`,
          description: 'Example of using UUIDs with Entity Framework Core.'
        },
        {
          id: 2,
          title: 'Dapper',
          code: `public async Task<User> GetUser(UUID id)
{
    using var connection = new SqlConnection(connectionString);
    return await connection.QuerySingleOrDefaultAsync<User>(
        "SELECT * FROM Users WHERE Id = @Id",
        new { Id = id.ToString() }
    );
}`,
          description: 'Example of using UUIDs with Dapper.'
        }
      ],

      webApiExamples: [
        {
          id: 1,
          title: 'ASP.NET Core Controller',
          code: `public class UserController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(UUID id)
    {
        var user = await _userService.GetUserAsync(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        user.Id = new UUID(); // Generate new UUID
        await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }
}`,
          description: 'Example of using UUIDs in ASP.NET Core Web API.'
        }
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
.examples {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #fff;
  animation: slideInDown 0.8s ease-out;
  position: relative;
}

h1::after {
  content: '';
  position: absolute;
  bottom: -10px;
  left: 0;
  width: 60px;
  height: 4px;
  background: #ffd700;
  animation: expandWidth 0.8s ease-out forwards;
}

h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #fff;
  border-bottom: 2px solid #ffd700;
  padding-bottom: 0.5rem;
  animation: slideInLeft 0.8s ease-out;
}

section {
  margin-bottom: 3rem;
  animation: fadeIn 1s ease-out;
}

.example-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 25px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  border: 1px solid #333;
  animation: slideInUp 0.8s ease-out;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.example-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 4px;
  height: 0;
  background: #ffd700;
  transition: height 0.3s ease;
}

.example-card:hover {
  transform: translateY(-4px) translateX(4px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
}

.example-card:hover::before {
  height: 100%;
}

.example-card h3 {
  color: #ffd700;
  margin-bottom: 1rem;
  font-size: 1.3rem;
  display: flex;
  align-items: center;
  gap: 10px;
  animation: fadeIn 0.8s ease-out;
}

.example-card p {
  color: #a7a7a7;
  line-height: 1.6;
  margin: 1rem 0;
  animation: slideInRight 0.8s ease-out;
}

.example-card code-block {
  margin: 1rem 0;
  position: relative;
  animation: slideInUp 0.6s ease-out;
  transition: transform 0.3s ease;
}

.example-card:hover code-block {
  transform: scale(1.01);
}

/* Animasyon tanımlamaları */
@keyframes slideInDown {
  from {
    opacity: 0;
    transform: translateY(-30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes slideInLeft {
  from {
    opacity: 0;
    transform: translateX(-30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes expandWidth {
  from {
    width: 0;
  }
  to {
    width: 60px;
  }
}

/* Kademeli görünme için gecikme sınıfları */
section:nth-child(2) { animation-delay: 0.1s; }
section:nth-child(3) { animation-delay: 0.2s; }
section:nth-child(4) { animation-delay: 0.3s; }
section:nth-child(5) { animation-delay: 0.4s; }

/* Responsive tasarım için medya sorguları */
@media (max-width: 768px) {
  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .example-card {
    padding: 20px;
  }

  .example-card h3 {
    font-size: 1.2rem;
  }
}
</style>