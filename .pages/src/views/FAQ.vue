<template>
  <div class="faq">
    <h1 class="animate-title">Frequently Asked Questions</h1>

    <section id="general" :class="{ 'animate-in': true }">
      <h2 class="animate-title">General Questions</h2>

      <div class="faq-grid">
        <div 
          v-for="(qa, index) in generalQuestions" 
          :key="'general-' + index"
          class="faq-card animate-card"
          :style="{ animationDelay: `${index * 0.1}s` }"
        >
          <h3><i class="fas fa-question-circle"></i> {{ qa.question }}</h3>
          <p>{{ qa.answer }}</p>
          <ul v-if="qa.list">
            <li v-for="(item, itemIndex) in qa.list" :key="'list-' + itemIndex">{{ item }}</li>
          </ul>
          <div v-if="qa.code" class="animate-code">
            <code-block>{{ qa.code }}</code-block>
          </div>
        </div>
      </div>
    </section>

    <section id="usage" :class="{ 'animate-in': true }" style="animation-delay: 0.3s">
      <h2 class="animate-title">Usage Questions</h2>

      <div class="faq-grid">
        <div 
          v-for="(qa, index) in usageQuestions" 
          :key="'usage-' + index"
          class="faq-card animate-card"
          :style="{ animationDelay: `${0.3 + (index * 0.1)}s` }"
        >
          <h3><i class="fas fa-question-circle"></i> {{ qa.question }}</h3>
          <code-block v-if="qa.code">{{ qa.code }}</code-block>
          <p v-if="qa.answer">{{ qa.answer }}</p>
          <ul v-if="qa.list">
            <li v-for="(item, itemIndex) in qa.list" :key="'list-' + itemIndex" v-html="item"></li>
          </ul>
          <div v-if="qa.code" class="animate-code">
            <code-block>{{ qa.code }}</code-block>
          </div>
        </div>
      </div>
    </section>

    <section id="performance" :class="{ 'animate-in': true }" style="animation-delay: 0.6s">
      <h2 class="animate-title">Performance Questions</h2>

      <div class="faq-grid">
        <div 
          v-for="(qa, index) in performanceQuestions" 
          :key="'perf-' + index"
          class="faq-card animate-card"
          :style="{ animationDelay: `${0.6 + (index * 0.1)}s` }"
        >
          <h3><i class="fas fa-question-circle"></i> {{ qa.question }}</h3>
          <p v-if="qa.answer">{{ qa.answer }}</p>
          <ul v-if="qa.list">
            <li v-for="(item, itemIndex) in qa.list" :key="'list-' + itemIndex">{{ item }}</li>
          </ul>
          <div v-if="qa.code" class="animate-code">
            <code-block>{{ qa.code }}</code-block>
          </div>
        </div>
      </div>
    </section>

    <section id="troubleshooting" :class="{ 'animate-in': true }" style="animation-delay: 0.9s">
      <h2 class="animate-title">Troubleshooting</h2>

      <div class="faq-grid">
        <div 
          v-for="(qa, index) in troubleshootingQuestions" 
          :key="'trouble-' + index"
          class="faq-card animate-card"
          :style="{ animationDelay: `${0.9 + (index * 0.1)}s` }"
        >
          <h3><i class="fas fa-question-circle"></i> {{ qa.question }}</h3>
          <p v-if="qa.answer">{{ qa.answer }}</p>
          <ul v-if="qa.list">
            <li v-for="(item, itemIndex) in qa.list" :key="'list-' + itemIndex">{{ item }}</li>
          </ul>
          <p v-if="qa.solution">{{ qa.solution }}</p>
          <div v-if="qa.code" class="animate-code">
            <code-block>{{ qa.code }}</code-block>
          </div>
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
  name: 'FAQ',
  components: {
    CodeBlock
  },
  data() {
    return {
      generalQuestions: [
        {
          question: 'What is UUID?',
          answer: 'UUID (Universally Unique Identifier) is a 128-bit identifier that\'s guaranteed to be unique across space and time. Our UUID library provides a high-performance, secure implementation for .NET applications.'
        },
        {
          question: 'How is UUID different from Guid?',
          answer: 'While UUID and Guid represent the same concept, our UUID implementation offers:',
          list: [
            'Better performance (up to 70% faster)',
            'More modern API design',
            'Additional string format options (Base32, Base64)',
            'Better security by default'
          ]
        },
        {
          question: 'Is UUID thread-safe?',
          answer: 'Yes, UUID operations are thread-safe. You can safely generate and manipulate UUIDs from multiple threads without any synchronization.'
        }
      ],
      usageQuestions: [
        {
          question: 'How do I generate a new UUID?',
          code: `// Simple generation
var uuid = new UUID();

// Generate multiple
UUID[] uuids = new UUID[1000];
ArrayExtension.Fill(uuids);`
        },
        {
          question: 'What string formats are supported?',
          code: `var uuid = new UUID();

// Standard format (default)
Console.WriteLine(uuid.ToString());
// "0123456789abcdef0123456789abcdef"

// Base32 (URL-safe)
Console.WriteLine(uuid.ToBase32());
// "028T5CY4TQKFF028T5CY4TQKFF"

// Base64
Console.WriteLine(uuid.ToBase64());
// "782riWdFIwHvzauJZ0UjAQ=="`,
          answer: 'UUID supports standard, Base32, and Base64 string formats.'
        },
        {
          question: 'How do I convert between different formats?',
          code: `var uuid = new UUID();

// Convert to Base64
string base64 = uuid.ToBase64();
UUID fromBase64 = UUID.FromBase64(base64);

// Convert to byte array
byte[] bytes = uuid.ToByteArray();
UUID fromBytes = UUID.FromByteArray(bytes);

// Convert to/from Guid (implicit)
Guid guid = uuid;
UUID fromGuid = guid;  // Implicit conversion`,
          answer: 'UUID supports various conversion methods with both standard and safe parsing options (Try* methods).'
        },
        {
          question: 'Which string format should I use?',
          list: [
            '<strong>Standard format:</strong> Best for human readability and debugging',
            '<strong>Base32:</strong> Best for URLs and file names (URL-safe, no special characters)',
            '<strong>Base64:</strong> Best for storage efficiency (shortest representation)'
          ]
        },
        {
          question: 'How do I handle conversion errors?',
          code: `// Safe Base64 parsing
if (UUID.TryFromBase64(base64String, out UUID uuid))
{
    // Success
    Console.WriteLine($"Parsed UUID: {uuid}");
}
else
{
    // Handle invalid input
    Console.WriteLine("Invalid Base64 string");
}

// Safe byte array parsing
if (UUID.TryFromByteArray(bytes, out UUID fromBytes))
{
    // Success
    Console.WriteLine($"Parsed UUID: {fromBytes}");
}
else
{
    // Handle invalid input
    Console.WriteLine("Invalid byte array");
}`,
          answer: 'Always use Try* methods when parsing untrusted input to handle errors gracefully.'
        },
        {
          question: 'How do I use UUIDs with Entity Framework?',
          code: `public class User
{
    public UUID Id { get; set; }
    public string Name { get; set; }
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .Property(e => e.Id)
        .HasConversion(
            v => v.ToByteArray(),
            v => new UUID(v));
}`
        }
      ],
      performanceQuestions: [
        {
          question: 'How many UUIDs can be generated per second?',
          answer: 'On a modern system:',
          list: [
            'Single thread: ~20.5M UUIDs/second',
            'Multi-thread (4 cores): ~78.2M UUIDs/second',
            'Multi-thread (8 cores): ~152.3M UUIDs/second'
          ]
        },
        {
          question: 'How can I improve UUID generation performance?',
          answer: 'For best performance:',
          list: [
            'Use bulk generation methods for multiple UUIDs',
            'Reuse UUID instances when possible',
            'Use TryParse instead of Parse',
            'Store UUIDs in binary format'
          ]
        }
      ],
      troubleshootingQuestions: [
        {
          question: 'Why am I getting a FormatException?',
          answer: 'FormatException occurs when parsing invalid UUID strings. Common issues:',
          list: [
            'Incorrect string format',
            'Missing or extra hyphens',
            'Invalid characters'
          ],
          solution: 'Solution: Use TryParse for safer parsing:',
          code: `if (UUID.TryParse(input, out var uuid))
{
    // Valid UUID
}
else
{
    // Invalid format
}`
        },
        {
          question: 'How do I migrate from Guid?',
          answer: 'UUID provides implicit conversion operators:',
          code: `// From Guid to UUID
Guid guid = Guid.NewGuid();
UUID uuid = guid;  // Implicit conversion

// From UUID to Guid
UUID uuid = new UUID();
Guid guid = uuid;  // Implicit conversion`
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
.faq {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

h1 {
  font-size: 2.5rem;
  margin-bottom: 2rem;
  color: #fff;
}

h2 {
  font-size: 2rem;
  margin-bottom: 1.5rem;
  color: #fff;
  border-bottom: 2px solid #ffd700;
  padding-bottom: 0.5rem;
}

section {
  margin-bottom: 3rem;
}

.faq-grid {
  display: grid;
  grid-template-columns: repeat(1, 1fr);
  gap: 20px;
}

.faq-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.faq-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
}

.faq-card h3 {
  color: #ffd700;
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 1rem;
  font-size: 1.3rem;
}

.faq-card p {
  color: #a7a7a7;
  line-height: 1.6;
  margin: 1rem 0;
}

.faq-card ul {
  list-style-type: none;
  padding: 0;
  margin: 1rem 0;
}

.faq-card li {
  color: #a7a7a7;
  margin: 0.5rem 0;
  padding-left: 1.5rem;
  position: relative;
  line-height: 1.6;
}

.faq-card li::before {
  content: '•';
  position: absolute;
  left: 0;
  color: #ffd700;
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

/* Prism.js custom color definitions */
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
  .faq {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .faq-card h3 {
    font-size: 1.2rem;
  }
}
</style>