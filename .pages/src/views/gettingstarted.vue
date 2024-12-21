<template>
  <div class="getting-started">
    <h1 class="animate-title">Getting Started</h1>

    <section id="installation" :class="{ 'animate-in': true }">
      <h2 class="animate-title">Installation</h2>
      <p class="animate-text">Install the UUID package via NuGet using one of the following methods:</p>
      <div class="animate-code">
        <code-block :language="'language-bash'">{{ installationCode }}</code-block>
      </div>
    </section>

    <section id="basic-usage" :class="{ 'animate-in': true }" style="animation-delay: 0.2s">
      <h2 class="animate-title">Basic Usage</h2>
      <p class="animate-text">Here's how to get started with basic UUID operations:</p>
      <div class="animate-code">
        <code-block>{{ basicUsageCode }}</code-block>
      </div>
    </section>

    <section id="string-formats" :class="{ 'animate-in': true }" style="animation-delay: 0.4s">
      <h2 class="animate-title">String Formats</h2>
      <p class="animate-text">UUID supports multiple string formats for different use cases:</p>
      <div class="animate-code">
        <code-block>{{ stringFormatsCode }}</code-block>
      </div>
    </section>

    <section id="bulk-operations" :class="{ 'animate-in': true }" style="animation-delay: 0.6s">
      <h2 class="animate-title">Bulk Operations</h2>
      <p class="animate-text">For high-performance scenarios where you need to generate multiple UUIDs efficiently:</p>
      <div class="animate-code">
        <code-block>{{ bulkOperationsCode }}</code-block>
      </div>

      <div class="info-box animate-box">
        <h3>Performance Tips</h3>
        <ul>
          <li>Use <code>Fill()</code> for existing arrays to avoid allocation overhead</li>
          <li>Use <code>TryGenerate()</code> for safe bulk generation with error handling</li>
          <li>All bulk operations are thread-safe and optimized for concurrent use</li>
        </ul>
      </div>
    </section>

    <section id="byte-operations" :class="{ 'animate-in': true }" style="animation-delay: 0.8s">
      <h2 class="animate-title">Byte Array Operations</h2>
      <p class="animate-text">Working with byte arrays is a common requirement. Here's how to do it efficiently:</p>
      <div class="animate-code">
        <code-block>{{ byteOperationsCode }}</code-block>
      </div>
    </section>

    <section id="error-handling" :class="{ 'animate-in': true }" style="animation-delay: 1s">
      <h2 class="animate-title">Error Handling</h2>
      <p class="animate-text">UUID provides methods for safe parsing and conversion with error handling:</p>
      <div class="animate-code">
        <code-block>{{ errorHandlingCode }}</code-block>
      </div>
    </section>

    <section id="performance-tips" :class="{ 'animate-in': true }" style="animation-delay: 1.2s">
      <h2 class="animate-title">Performance Tips</h2>
      <div class="tip-cards">
        <div 
          v-for="(tip, index) in performanceTips" 
          :key="index"
          class="tip-card animate-card"
          :style="{ animationDelay: `${1.4 + (index * 0.2)}s` }"
        >
          <h3><i :class="tip.icon"></i> {{ tip.title }}</h3>
          <ul>
            <li v-for="(item, itemIndex) in tip.items" :key="itemIndex">{{ item }}</li>
          </ul>
        </div>
      </div>
    </section>

    <section id="next-steps" :class="{ 'animate-in': true }" style="animation-delay: 2s">
      <h2 class="animate-title">Next Steps</h2>
      <div class="next-steps-grid">
        <router-link 
          v-for="(step, index) in nextSteps" 
          :key="index"
          :to="step.link"
          class="next-step-card animate-card"
          :style="{ animationDelay: `${2.2 + (index * 0.2)}s` }"
        >
          <i :class="step.icon"></i>
          <h3>{{ step.title }}</h3>
          <p>{{ step.description }}</p>
        </router-link>
      </div>
    </section>
  </div>
</template>

<script>
import Prism from 'prismjs'
import 'prismjs/themes/prism-tomorrow.css'
import 'prismjs/components/prism-bash'
import 'prismjs/components/prism-csharp'
import CodeBlock from '@/components/CodeBlock.vue'

export default {
  name: 'GettingStarted',
  components: {
    CodeBlock
  },
  data() {
    return {
      installationCode: `# .NET CLI
dotnet add package UUID

# Package Manager Console
Install-Package UUID`,

      basicUsageCode: `// Generate a new UUID
var id = new UUID();

// Convert to string
string str = id.ToString();  // "0123456789abcdef0123456789abcdef"

// Parse from string
UUID parsed = UUID.Parse("0123456789abcdef0123456789abcdef");

// Check UUID version and variant
Console.WriteLine($"Version: {id.Version}"); // 7 (UUIDv7)
Console.WriteLine($"Variant: {id.Variant}"); // 2 (RFC 4122)

// Get timestamp from UUID
Console.WriteLine($"Time: {id.Time:yyyy-MM-dd HH:mm:ss.fff}");`,

      stringFormatsCode: `var uuid = new UUID();

// Standard format
string standard = uuid.ToString();
// "0123456789abcdef0123456789abcdef"

// Int64 format
long int64 = uuid.ToInt64();
// "40992764608247672"

// Base32 format (URL-safe)
string base32 = uuid.ToBase32();
// "028T5CY4TQKFF028T5CY4TQKFF"

// Base64 format
string base64 = uuid.ToBase64();
// "782riWdFIwHvzauJZ0UjAQ=="

// Convert Base64 back to UUID
UUID fromBase64 = UUID.FromBase64(base64);
Console.WriteLine($"Base64 -> UUID: {fromBase64}");

// Safe parsing with TryFromBase64
if (UUID.TryFromBase64(base64, out UUID parsedFromBase64))
{
    Console.WriteLine($"Successfully parsed from Base64: {parsedFromBase64}");
}`,

      bulkOperationsCode: `// Fill an existing array with UUIDs
UUID[] uuids = new UUID[1000];
uuids.Fill();  // Thread-safe, efficient bulk generation

// Generate a new array of UUIDs
UUID[] generated = ArrayExtension.Generate(1000);

// With error handling
if (ArrayExtension.TryGenerate(1000, out UUID[]? result))
{
    foreach (var uuid in result)
    {
        // Process each UUID
        Console.WriteLine(uuid);
    }
}`,

      byteOperationsCode: `// Convert UUID to byte array
UUID id = UUID.New();
byte[] bytes = id.ToByteArray();
Console.WriteLine($"UUID -> Bytes: {BitConverter.ToString(bytes)}");

// Convert byte array back to UUID
UUID fromBytes = UUID.FromByteArray(bytes);
Console.WriteLine($"Bytes -> UUID: {fromBytes}");

// Safe parsing with TryFromByteArray
if (UUID.TryFromByteArray(bytes, out UUID parsedFromBytes))
{
    Console.WriteLine($"Successfully parsed from bytes: {parsedFromBytes}");
}

// Writing directly to a byte array
byte[] destination = new byte[16];
if (id.TryWriteBytes(destination))
{
    Console.WriteLine($"Successfully wrote to byte array: {BitConverter.ToString(destination)}");
}`,

      errorHandlingCode: `// Safe parsing with TryParse
if (UUID.TryParse("0123456789abcdef0123456789abcdef", out UUID result))
{
    Console.WriteLine($"Successfully parsed: {result}");
}

// Safe byte array conversion
byte[] bytes = new byte[16];
if (UUID.TryFromByteArray(bytes, out UUID fromBytes))
{
    Console.WriteLine($"Successfully converted: {fromBytes}");
}

// Safe Base64 conversion
if (UUID.TryFromBase64("AAAAAAAAAAAAAAAAAAAAAA==", out UUID fromBase64))
{
    Console.WriteLine($"Successfully converted: {fromBase64}");
}`,

      performanceTips: [
        {
          icon: 'fas fa-lightbulb',
          title: 'String Formatting',
          items: [
            'Use appropriate string format methods based on your needs',
            'Consider using Base32 for URL-safe strings'
          ]
        },
        {
          icon: 'fas fa-server',
          title: 'Bulk Operations',
          items: [
            'Use array pooling for bulk operations',
            'Consider parallel processing for large batches'
          ]
        },
        {
          icon: 'fas fa-memory',
          title: 'Memory Usage',
          items: [
            'Reuse UUID instances when possible',
            'Use pooled arrays for bulk operations',
            'Consider using Span<T> for zero-allocation parsing'
          ]
        }
      ],

      nextSteps: [
        {
          icon: 'fas fa-code',
          title: 'View Examples',
          description: 'See more usage scenarios and code samples',
          link: '/examples'
        },
        {
          icon: 'fas fa-book',
          title: 'API Reference',
          description: 'Explore the complete API documentation',
          link: '/api-reference'
        },
        {
          icon: 'fas fa-tachometer-alt',
          title: 'Performance Guide',
          description: 'Learn about optimization techniques',
          link: '/performance'
        }
      ]
    }
  },
  mounted() {
    Prism.highlightAll()
  }
}
</script>

<style scoped>
.getting-started {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
}

section {
  margin-bottom: 3rem;
}

h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #fff;
}

p {
  font-size: 1.1rem;
  line-height: 1.6;
  color: #a7a7a7;
  margin-bottom: 1.5rem;
}

.info-box {
  background-color: rgba(255, 215, 0, 0.1);
  border-left: 4px solid #ffd700;
  padding: 20px;
  border-radius: 4px;
  margin: 20px 0;
}

.info-box h3 {
  color: #ffd700;
  margin-bottom: 10px;
}

.info-box ul {
  margin: 0;
  padding-left: 20px;
}

.info-box li {
  color: #a7a7a7;
  margin-bottom: 8px;
}

.tip-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
  margin-top: 20px;
}

.tip-card {
  background-color: #1e1e1e;
  padding: 20px;
  border-radius: 8px;
}

.tip-card h3 {
  color: #ffd700;
  margin-bottom: 15px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.tip-card i {
  font-size: 1.2rem;
}

.tip-card ul {
  margin: 0;
  padding-left: 20px;
}

.tip-card li {
  color: #a7a7a7;
  margin-bottom: 8px;
  line-height: 1.4;
}

.next-steps-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-top: 20px;
}

.next-step-card {
  background-color: #1e1e1e;
  padding: 25px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.3s ease;
  text-align: center;
}

.next-step-card:hover {
  transform: translateY(-5px);
  background-color: rgba(255, 255, 255, 0.1);
}

.next-step-card i {
  font-size: 2rem;
  color: #ffd700;
  margin-bottom: 15px;
}

.next-step-card h3 {
  color: #fff;
  margin: 10px 0;
  font-size: 1.2rem;
}

.next-step-card p {
  color: #a7a7a7;
  margin: 0;
  font-size: 0.9rem;
}

code {
  background-color: rgba(255, 255, 255, 0.1);
  padding: 2px 6px;
  border-radius: 4px;
  font-family: 'Consolas', 'Monaco', monospace;
}

.animate-in {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards;
}

.animate-title {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards;
}

.animate-text {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.2s;
}

.animate-code {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.3s;
}

.animate-box {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.4s;
}

.animate-card {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards;
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.tip-card, .next-step-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.tip-card:hover, .next-step-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
}

.tip-card i, .next-step-card i {
  transition: transform 0.3s ease;
}

.tip-card:hover i, .next-step-card:hover i {
  transform: scale(1.1);
}

@media (max-width: 768px) {
  .getting-started {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .tip-cards {
    grid-template-columns: 1fr;
  }

  .next-steps-grid {
    grid-template-columns: 1fr;
  }
}
</style>