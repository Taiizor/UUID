<template>
  <div class="security">
    <h1>Security Guide</h1>

    <section id="overview">
      <h2>Security Overview</h2>
      <p>UUID library is designed with security in mind, using cryptographically secure random number generation by default.</p>

      <div class="security-card">
        <h3><i class="fas fa-shield-alt"></i> Key Security Features</h3>
        <ul>
          <li v-for="(feature, index) in securityFeatures" :key="'feature-' + index">{{ feature }}</li>
        </ul>
      </div>
    </section>

    <section id="random-generation">
      <h2>Random Number Generation</h2>

      <div class="security-card">
        <h3><i class="fas fa-random"></i> Secure Generation</h3>
        <p>UUID uses System.Security.Cryptography.RandomNumberGenerator for secure random number generation:</p>
        <code-block>{{ secureGeneration.code }}</code-block>
      </div>
    </section>

    <section id="best-practices">
      <h2>Security Best Practices</h2>

      <div class="practice-card">
        <h3><i class="fas fa-check-circle"></i> Recommended Practices</h3>
        <ul>
          <li v-for="(practice, index) in recommendedPractices" :key="'rec-' + index">{{ practice }}</li>
        </ul>
      </div>

      <div class="practice-card">
        <h3><i class="fas fa-times-circle"></i> Practices to Avoid</h3>
        <ul>
          <li v-for="(practice, index) in practicestoAvoid" :key="'avoid-' + index">{{ practice }}</li>
        </ul>
      </div>
    </section>

    <section id="input-validation">
      <h2>Input Validation</h2>

      <div class="security-card">
        <h3><i class="fas fa-check"></i> Safe Parsing</h3>
        <p>Always use TryParse for untrusted input:</p>
        <code-block>{{ inputValidation.code }}</code-block>
      </div>
    </section>

    <section id="storage">
      <h2>Secure Storage</h2>

      <div class="security-card">
        <h3><i class="fas fa-database"></i> Database Storage</h3>
        <p>Best practices for storing UUIDs in databases:</p>
        <code-block>{{ databaseStorage.code }}</code-block>
      </div>

      <div class="security-card">
        <h3><i class="fas fa-code"></i> Entity Framework</h3>
        <p>Configure EF Core for secure UUID handling:</p>
        <code-block>{{ entityFramework.code }}</code-block>
      </div>
    </section>

    <section id="cryptographic-notes">
      <h2>Cryptographic Notes</h2>

      <div class="security-card warning">
        <h3><i class="fas fa-exclamation-triangle"></i> Important Security Notes</h3>
        <ul>
          <li v-for="(note, index) in securityNotes" :key="'note-' + index">{{ note }}</li>
        </ul>
      </div>
    </section>
  </div>
</template>

<script>
import Prism from 'prismjs'
import 'prismjs/themes/prism-tomorrow.css'
import 'prismjs/components/prism-csharp'
import 'prismjs/components/prism-sql'
import CodeBlock from '@/components/CodeBlock.vue'

export default {
  name: 'Security',
  components: {
    CodeBlock
  },
  data() {
    return {
      securityFeatures: [
        'Cryptographically secure random number generation',
        'Protection against timing attacks',
        'Safe string parsing and validation',
        'Thread-safe operations'
      ],

      secureGeneration: {
        code: `// Automatically uses secure RNG
var uuid = new UUID();

// For bulk operations
UUID[] uuids = new UUID[1000];
ArrayExtension.Fill(uuids); // Still uses secure RNG`
      },

      recommendedPractices: [
        'Always validate UUID inputs',
        'Use TryParse for untrusted input',
        'Store UUIDs in their binary form when possible',
        'Use URL-safe Base32 encoding for web contexts'
      ],

      practicestoAvoid: [
        'Don\'t use UUIDs for sensitive data encoding',
        'Don\'t use UUIDs as security tokens',
        'Don\'t assume sequential UUIDs are secure',
        'Don\'t expose internal UUID representation'
      ],

      inputValidation: {
        code: `// Safe parsing of untrusted input
public bool ValidateUserInput(string input)
{
    if (UUID.TryParse(input, out var uuid))
    {
        // Input is a valid UUID
        ProcessValidUUID(uuid);
        return true;
    }
    return false;
}`
      },

      databaseStorage: {
        code: `-- SQL Server
CREATE TABLE Users (
    Id BINARY(16) PRIMARY KEY,  -- Most efficient
    -- or
    Id UNIQUEIDENTIFIER PRIMARY KEY  -- If you need native UUID type
);`
      },

      entityFramework: {
        code: `protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .Property(e => e.Id)
        .HasConversion(
            v => v.ToByteArray(),  // Store as binary
            v => new UUID(v));
}`
      },

      securityNotes: [
        'UUIDs are not suitable for cryptographic purposes',
        'Do not use UUIDs to store sensitive information',
        'UUIDs are not guaranteed to be unique across systems',
        'Use proper cryptographic functions for security-critical operations'
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
.security {
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

.security-card, .practice-card {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 1.5rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  animation: slideInRight 0.8s ease-out;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.security-card:hover, .practice-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);
}

.security-card h3, .practice-card h3 {
  color: #ffd700;
  margin-bottom: 1rem;
  display: flex;
  align-items: center;
  gap: 10px;
  animation: fadeIn 1s ease-out;
}

.security-card i, .practice-card i {
  animation: rotateIn 0.8s ease-out;
}

.security-card ul li, .practice-card ul li {
  animation: slideInLeft 0.6s ease-out;
  animation-fill-mode: both;
}

.security-card ul li:nth-child(1) { animation-delay: 0.1s; }
.security-card ul li:nth-child(2) { animation-delay: 0.2s; }
.security-card ul li:nth-child(3) { animation-delay: 0.3s; }
.security-card ul li:nth-child(4) { animation-delay: 0.4s; }

.warning {
  border-left: 4px solid #ffd700;
  animation: pulseWarning 2s infinite;
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

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes rotateIn {
  from {
    opacity: 0;
    transform: rotate(-180deg);
  }
  to {
    opacity: 1;
    transform: rotate(0);
  }
}

@keyframes pulseWarning {
  0% {
    border-color: #ffd700;
  }
  50% {
    border-color: #ff6b6b;
  }
  100% {
    border-color: #ffd700;
  }
}

p {
  color: #a7a7a7;
  line-height: 1.6;
  margin-bottom: 1.5rem;
}

.security-card ul, .practice-card ul {
  list-style-type: none;
  padding: 0;
}

.security-card li, .practice-card li {
  color: #a7a7a7;
  margin: 0.5rem 0;
  padding-left: 1.5rem;
  position: relative;
  line-height: 1.6;
}

.security-card li::before, .practice-card li::before {
  content: '•';
  position: absolute;
  left: 0;
  color: #ffd700;
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
  .security {
    padding: 20px;
  }

  h1 {
    font-size: 2rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .security-card h3, .practice-card h3 {
    font-size: 1.2rem;
  }

  .code-block {
    padding: 15px;
  }
}
</style>