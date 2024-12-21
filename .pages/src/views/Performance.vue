<template>
  <div class="performance">
    <h1>Performance Guide</h1>

    <section id="benchmarks">
      <h2>Benchmarks</h2>

      <div class="benchmark-grid">
        <div v-for="(benchmark, index) in benchmarks" :key="'bench-' + index" class="benchmark-card">
          <h3>{{ benchmark.title }}</h3>
          <p class="number">{{ benchmark.value }}</p>
          <p class="label">{{ benchmark.label }}</p>
        </div>
      </div>
    </section>

    <section id="optimization-tips">
      <h2>Optimization Tips</h2>

      <div v-for="(tip, index) in optimizationTips" :key="'tip-' + index" class="tip-card">
        <h3><i :class="tip.icon"></i> {{ tip.title }}</h3>
        <p>{{ tip.description }}</p>
        <div class="code-block">
          <code-block>{{ tip.code }}</code-block>
        </div>
      </div>
    </section>

    <section id="comparison">
      <h2>Performance Comparison</h2>

      <div class="comparison-table">
        <table>
          <thead>
            <tr>
              <th v-for="header in comparisonHeaders" :key="header">{{ header }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(row, index) in comparisonData" :key="'row-' + index">
              <td>{{ row.operation }}</td>
              <td>{{ row.uuid }}</td>
              <td>{{ row.guid }}</td>
              <td class="improvement">{{ row.improvement }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </section>

    <section id="best-practices">
      <h2>Best Practices</h2>

      <div class="practice-card">
        <h3><i class="fas fa-check-circle"></i> Do</h3>
        <ul>
          <li v-for="(practice, index) in doPractices" :key="'do-' + index">{{ practice }}</li>
        </ul>
      </div>

      <div class="practice-card">
        <h3><i class="fas fa-times-circle"></i> Don't</h3>
        <ul>
          <li v-for="(practice, index) in dontPractices" :key="'dont-' + index">{{ practice }}</li>
        </ul>
      </div>
    </section>

    <section id="profiling">
      <h2>Profiling Tools</h2>

      <div class="tool-card">
        <h3>{{ profilingTool.title }}</h3>
        <p>{{ profilingTool.description }}</p>
        <div class="code-block">
          <code-block>{{ profilingTool.code }}</code-block>
        </div>
      </div>
    </section>

    <div class="code-example" v-for="example in examples" :key="example.id">
      <h3>{{ example.title }}</h3>
      <p>{{ example.description }}</p>
      <code-block>{{ example.code }}</code-block>
    </div>
  </div>
</template>

<script>
import Prism from 'prismjs'
import 'prismjs/themes/prism-tomorrow.css'
import 'prismjs/components/prism-csharp'
import CodeBlock from '@/components/CodeBlock.vue'

export default {
  name: 'Performance',
  components: {
    CodeBlock
  },
  data() {
    return {
      benchmarks: [
        {
          title: 'Generation',
          value: '20.5M',
          label: 'UUIDs/second (single thread)'
        },
        {
          title: 'Parsing',
          value: '15.8M',
          label: 'UUIDs/second'
        },
        {
          title: 'ToString',
          value: '12.3M',
          label: 'conversions/second'
        },
        {
          title: 'Memory',
          value: '16',
          label: 'bytes per UUID'
        }
      ],

      optimizationTips: [
        {
          title: 'Bulk Operations',
          icon: 'fas fa-bolt',
          description: 'When generating multiple UUIDs, use the array methods:',
          code: `// More efficient than generating individually
UUID[] uuids = new UUID[1000];
ArrayExtension.Fill(uuids);`
        },
        {
          title: 'Memory Management',
          icon: 'fas fa-memory',
          description: 'Use the zero-allocation methods when possible:',
          code: `// Avoid allocations
Span<byte> buffer = stackalloc byte[16];
uuid.TryWriteBytes(buffer);`
        },
        {
          title: 'Parallel Processing',
          icon: 'fas fa-sync',
          description: 'Take advantage of parallel processing for bulk operations:',
          code: `// Process UUIDs in parallel
Parallel.ForEach(uuids, uuid =>
{
    ProcessUUID(uuid);
});`
        },
        {
          title: 'String Conversions',
          icon: 'fas fa-exchange-alt',
          description: 'Choose the right string format for your use case:',
          code: `// Base32 is URL-safe and compact
string base32 = uuid.ToBase32(); // Most efficient for URLs

// Base64 is compact but may need URL encoding
string base64 = uuid.ToBase64(); // Most space-efficient

// Standard format is human-readable
string standard = uuid.ToString(); // Most readable`
        },
        {
          title: 'Byte Array Operations',
          icon: 'fas fa-microchip',
          description: 'Use the most efficient byte array methods:',
          code: `// Pre-allocate buffer for multiple writes
byte[] buffer = new byte[16];
if (uuid.TryWriteBytes(buffer))
{
    // Use buffer directly without allocation
    ProcessBytes(buffer);
}

// For single use, ToByteArray is simpler
byte[] bytes = uuid.ToByteArray();`
        },
        {
          title: 'Guid Conversions',
          icon: 'fas fa-random',
          description: 'Take advantage of implicit conversions:',
          code: `// Implicit conversion is most efficient
UUID uuid = new UUID();
Guid guid = uuid; // No explicit conversion needed

// Avoid unnecessary conversions
void ProcessId(UUID uuid)
{
    // Don't convert if UUID is accepted
    ProcessUUID(uuid);
    
    // Convert only when Guid is required
    ProcessGuid(uuid); // Implicit conversion
}`
        }
      ],

      comparisonHeaders: ['Operation', 'UUID', 'Guid', 'Improvement'],
      comparisonData: [
        {
          operation: 'Generation',
          uuid: '20.5M/s',
          guid: '12.3M/s',
          improvement: '+66%'
        },
        {
          operation: 'Parse',
          uuid: '15.8M/s',
          guid: '8.9M/s',
          improvement: '+77%'
        },
        {
          operation: 'ToString',
          uuid: '12.3M/s',
          guid: '7.2M/s',
          improvement: '+70%'
        }
      ],

      doPractices: [
        'Use array pooling for bulk operations',
        'Take advantage of Span<T> APIs',
        'Use TryParse over Parse when possible',
        'Cache frequently used UUIDs'
      ],

      dontPractices: [
        'Generate UUIDs in tight loops individually',
        'Convert to string unnecessarily',
        'Parse the same UUID repeatedly',
        'Ignore the available bulk operations'
      ],

      profilingTool: {
        title: 'BenchmarkDotNet',
        description: 'Our benchmarks are created using BenchmarkDotNet. You can run them yourself:',
        code: `public class UUIDBenchmarks
{
    [Benchmark]
    public void GenerateUUID()
    {
        var uuid = new UUID();
    }

    [Benchmark]
    public void ParseUUID()
    {
        UUID.Parse("0123456789abcdef0123456789abcdef");
    }
}`
      }
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
.performance {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #fff;
  animation: slideInDown 0.8s ease-out;
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

.benchmark-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin: 2rem 0;
  animation: fadeIn 1s ease-out;
}

.benchmark-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  text-align: center;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: scaleIn 0.6s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.benchmark-card:hover {
  transform: translateY(-4px) scale(1.02);
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
}

.benchmark-card h3 {
  color: #ffd700;
  margin-bottom: 1rem;
  animation: fadeIn 0.8s ease-out;
}

.benchmark-card .number {
  font-size: 2.5rem;
  font-weight: bold;
  color: #fff;
  margin: 0.5rem 0;
  animation: countUp 1s ease-out;
}

.tip-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: slideInRight 0.8s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.tip-card:hover {
  transform: translateX(4px);
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}

.comparison-table {
  overflow-x: auto;
  margin: 2rem 0;
  animation: fadeIn 1s ease-out;
  border-radius: 8px;
  background: #1e1e1e;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.comparison-table table {
  width: 100%;
  border-collapse: collapse;
  border-radius: 8px;
  animation: slideInUp 0.8s ease-out;
}

.comparison-table th,
.comparison-table td {
  padding: 15px 20px;
  text-align: left;
  border-bottom: 1px solid #333;
}

.comparison-table th {
  background-color: #252525;
  color: #ffd700;
  font-weight: 600;
  font-size: 1.1rem;
}

.comparison-table td {
  color: #d4d4d4;
  font-size: 1.1rem;
}

.comparison-table tr:last-child td {
  border-bottom: none;
}

.comparison-table tr {
  transition: background-color 0.3s ease;
}

.comparison-table tr:hover {
  background-color: #252525;
}

.improvement {
  color: #4caf50 !important;
  font-weight: bold;
  animation: pulse 2s infinite;
  padding: 4px 8px;
  border-radius: 4px;
  background: rgba(76, 175, 80, 0.1);
}

.practice-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 25px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: slideInLeft 0.8s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  border: 1px solid #333;
}

.practice-card h3 {
  color: #ffd700;
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 1.2rem;
  font-size: 1.3rem;
  font-weight: 600;
}

.practice-card ul {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

.practice-card li {
  color: #d4d4d4;
  margin: 12px 0;
  padding-left: 25px;
  position: relative;
  line-height: 1.6;
  font-size: 1.1rem;
  animation: slideInLeft 0.6s ease-out;
  animation-fill-mode: both;
}

.practice-card li::before {
  content: '•';
  position: absolute;
  left: 8px;
  color: #ffd700;
  font-size: 1.2rem;
}

.practice-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
}

.fas.fa-check-circle {
  color: #4caf50;
}

.fas.fa-times-circle {
  color: #ff5252;
}

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

@keyframes scaleIn {
  from {
    opacity: 0;
    transform: scale(0.9);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}

@keyframes countUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes pulse {
  0% {
    opacity: 1;
  }
  50% {
    opacity: 0.7;
  }
  100% {
    opacity: 1;
  }
}

/* Prism.js özel renk tanımlamaları */
:deep(.token.comment),
:deep(.token.prolog),
:deep(.token.doctype),
:deep(.token.cdata) {
  color: #6a9955;
}

:deep(.token.punctuation) {
  color: #d4d4d4;
}

:deep(.token.property),
:deep(.token.keyword),
:deep(.token.tag) {
  color: #569cd6;
}

:deep(.token.string) {
  color: #ce9178;
}

:deep(.token.operator),
:deep(.token.entity),
:deep(.token.url) {
  color: #d4d4d4;
}

:deep(.token.variable),
:deep(.token.function) {
  color: #dcdcaa;
}

:deep(.token.number) {
  color: #b5cea8;
}

@media (max-width: 768px) {
  .performance {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .benchmark-card .number {
    font-size: 2rem;
  }

  .code-block {
    padding: 15px;
  }

  .comparison-table {
    margin: 1rem -20px;
  }
}
</style>