<template>
  <div class="api-reference">
    <h1 class="animate-title">API Reference</h1>

    <section id="uuid-struct" :class="{ 'animate-in': true }">
      <h2 class="animate-title">UUID Struct</h2>
      <p class="animate-text">The core struct representing a UUID value.</p>

      <div class="code-block animate-code">
        <code-block>{{ uuidStructCode }}</code-block>
      </div>

      <h3 class="animate-title">Properties</h3>
      <div class="api-table">
        <table>
          <thead>
            <tr>
              <th>Property</th>
              <th>Type</th>
              <th>Description</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(prop, index) in properties" :key="index">
              <td>{{ prop.name }}</td>
              <td>{{ prop.type }}</td>
              <td>{{ prop.description }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <h3 class="animate-title">Constructors</h3>
      <div class="code-block animate-code">
        <code-block>{{ constructorsCode }}</code-block>
      </div>
    </section>

    <section id="methods" :class="{ 'animate-in': true }" style="animation-delay: 0.3s">
      <h2 class="animate-title">Methods</h2>

      <h3 class="animate-title">String Formatting</h3>
      <div class="code-block animate-code">
        <code-block>{{ stringFormattingCode }}</code-block>
      </div>

      <h3 class="animate-title">Byte Array Operations</h3>
      <div class="code-block animate-code">
        <code-block>{{ byteArrayCode }}</code-block>
      </div>

      <h3 class="animate-title">Guid Operations</h3>
      <div class="code-block animate-code">
        <code-block>{{ guidOperationsCode }}</code-block>
      </div>

      <h3 class="animate-title">Comparison Methods</h3>
      <div class="code-block animate-code">
        <code-block>{{ comparisonMethodsCode }}</code-block>
      </div>
    </section>

    <section id="extensions" :class="{ 'animate-in': true }" style="animation-delay: 0.6s">
      <h2 class="animate-title">Extension Methods</h2>
      <div class="code-block animate-code">
        <code-block>{{ extensionMethodsCode }}</code-block>
      </div>
    </section>

    <section id="exceptions" :class="{ 'animate-in': true }" style="animation-delay: 0.9s">
      <h2 class="animate-title">Exceptions</h2>
      <div class="exceptions-grid">
        <div 
          v-for="(exception, index) in exceptions" 
          :key="index"
          class="tip-card animate-card"
          :style="{ animationDelay: `${0.9 + (index * 0.1)}s` }"
        >
          <h3><i class="fas fa-exclamation-triangle"></i> {{ exception.name }}</h3>
          <p>Thrown when:</p>
          <ul>
            <li v-for="(reason, reasonIndex) in exception.reasons" :key="reasonIndex">
              {{ reason }}
            </li>
          </ul>
        </div>
      </div>
    </section>

    <div class="code-example animate-code" v-for="example in examples" :key="example.id">
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
  name: 'ApiReference',
  components: {
    CodeBlock
  },
  data() {
    return {
      uuidStructCode: 'public readonly partial struct UUID : IEquatable<UUID>, IComparable<UUID>, IComparable',

      properties: [
        {
          name: 'Empty',
          type: 'UUID',
          description: 'Gets the empty UUID value (all zeros)'
        },
        {
          name: 'Version',
          type: 'byte',
          description: 'Gets the UUID version number (7 for UUIDv7)'
        },
        {
          name: 'Variant',
          type: 'byte',
          description: 'Gets the UUID variant number (2 for RFC 4122)'
        },
        {
          name: 'Time',
          type: 'DateTimeOffset',
          description: 'Gets the timestamp embedded in the UUID (UUIDv7 only)'
        }
      ],

      constructorsCode: `// Create a new random UUID
public UUID()

// Create from bytes
public UUID(ReadOnlySpan<byte> bytes)

// Create from string
public static UUID Parse(string input)
public static UUID Parse(ReadOnlySpan<char> input)

// Try parse from string
public static bool TryParse(string input, out UUID result)
public static bool TryParse(ReadOnlySpan<char> input, out UUID result)`,

      stringFormattingCode: `// Convert to standard string format
public override string ToString()

// Convert to Base32 format
public string ToBase32()

// Convert to Base64 format
public string ToBase64()

// Convert from Base64 format
public static UUID FromBase64(string base64)
public static bool TryFromBase64(string base64, out UUID result)`,

      byteArrayCode: `// Convert to byte array
public byte[] ToByteArray()
public bool TryWriteBytes(byte[] destination)
public bool TryWriteBytes(Span<byte> destination)

// Convert from byte array
public static UUID FromByteArray(byte[] bytes)
public static bool TryFromByteArray(byte[] bytes, out UUID result)`,

      guidOperationsCode: `// Convert to/from Guid (implicit operators)
public static implicit operator Guid(UUID uuid)
public static implicit operator UUID(Guid guid)

// Explicit conversion methods
public Guid ToGuid()
public static UUID FromGuid(Guid guid)`,

      comparisonMethodsCode: `// Standard equality methods
public bool Equals(UUID other)
public override bool Equals(object obj)
public override int GetHashCode()

// Comparison methods (implements IComparable)
public int CompareTo(UUID other)
public int CompareTo(object obj)

// Comparison operators
public static bool operator ==(UUID left, UUID right)
public static bool operator !=(UUID left, UUID right)
public static bool operator <(UUID left, UUID right)
public static bool operator >(UUID left, UUID right)
public static bool operator <=(UUID left, UUID right)
public static bool operator >=(UUID left, UUID right)

// UUIDv7 specific comparisons
public bool IsOrderedAfter(UUID other)
public static int CompareTimestamps(UUID a, UUID b)
public static bool AreMonotonicallyOrdered(UUID[] uuids)`,

      extensionMethodsCode: `// Array extensions
public static class ArrayExtension
{
    // Generate array of UUIDs
    public static UUID[] Generate(int count)
    
    // Try generate array of UUIDs
    public static bool TryGenerate(int count, out UUID[]? result)
    
    // Fill array with UUIDs
    public static void Fill(this UUID[] array)
    
    // Try fill array with UUIDs
    public static bool TryFill(this UUID[] array)
}`,

      exceptions: [
        {
          name: 'FormatException',
          reasons: [
            'Invalid string format in Parse methods',
            'Invalid byte array length in constructor'
          ]
        },
        {
          name: 'ArgumentNullException',
          reasons: [
            'Null input string in Parse methods',
            'Null byte array in constructor'
          ]
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
.api-reference {
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

h3 {
  font-size: 1.5rem;
  margin: 2rem 0 1rem;
  color: #ffd700;
}

p {
  font-size: 1.1rem;
  line-height: 1.6;
  color: #a7a7a7;
  margin-bottom: 1.5rem;
}

.api-table {
  margin: 2rem 0;
  overflow-x: auto;
}

.api-table table {
  width: 100%;
  border-collapse: collapse;
  background: #1a1a1a;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.api-table th {
  background: #2a2a2a;
  color: #ffd700;
  font-weight: 600;
  text-align: left;
  padding: 1rem;
  border-bottom: 2px solid #333;
}

.api-table td {
  padding: 1rem;
  color: #fff;
  border-bottom: 1px solid #333;
}

.api-table tr:last-child td {
  border-bottom: none;
}

.api-table tr:hover td {
  background: #2a2a2a;
}

.api-table th:first-child,
.api-table td:first-child {
  padding-left: 1.5rem;
}

.api-table th:last-child,
.api-table td:last-child {
  padding-right: 1.5rem;
}

.exceptions-grid {
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
  font-size: 1.2rem;
}

.tip-card i {
  color: #ffd700;
}

.tip-card p {
  margin-bottom: 10px;
  font-size: 1rem;
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

/* Animasyonlar */
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

.animate-card {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards;
}

.animate-code {
  opacity: 0;
  transform: translateY(10px);
  animation: fadeInUp 0.4s ease forwards 0.2s;
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

/* Hover Animasyonları */
.tip-card {
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  position: relative;
  overflow: hidden;
}

.tip-card::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 215, 0, 0.1),
    transparent
  );
  transition: 0.5s;
}

.tip-card:hover::before {
  left: 100%;
}

.tip-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
}

.header i {
  animation: pulse 2s infinite;
}

@keyframes pulse {
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}

/* Mobil Responsive */
@media (max-width: 768px) {
  .api-reference {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  h3 {
    font-size: 1.2rem;
  }

  .exceptions-grid {
    grid-template-columns: 1fr;
  }

  table {
    display: block;
    overflow-x: auto;
  }
}
</style>
