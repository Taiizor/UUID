<template>
  <div class="home">
    <div class="hero" :class="{ 'animate-in': true }">
      <h1 class="animate-title">Modern UUID Generator for .NET</h1>
      <p class="animate-subtitle">High-performance, secure, and thread-safe UUID implementation</p>
      <div class="cta-buttons animate-buttons">
        <router-link to="/getting-started" class="btn primary">Get Started</router-link>
        <a href="https://github.com/Taiizor/UUID" target="_blank" rel="noopener" class="btn secondary">View on GitHub</a>
      </div>
    </div>

    <section class="features">
      <h2 class="animate-title">Features</h2>
      <div class="feature-grid">
        <div 
          v-for="(feature, index) in features" 
          :key="index"
          class="feature-card"
          :class="{ 'animate-in': true }"
          :style="{ animationDelay: `${index * 0.1}s` }"
        >
          <i :class="[feature.icon, 'animate-icon']"></i>
          <h3>{{ feature.title }}</h3>
          <p>{{ feature.description }}</p>
        </div>
      </div>
    </section>

    <section class="code-example" :class="{ 'animate-in': true }">
      <h2 class="animate-title">Quick Start</h2>
      <div class="animate-code">
        <code-block>{{ quickStartCode }}</code-block>
      </div>
    </section>

    <section class="benchmarks">
      <h2 class="animate-title">Performance Benchmarks</h2>
      <div class="benchmark-grid">
        <div 
          v-for="(benchmark, index) in benchmarks" 
          :key="index"
          class="benchmark-card"
          :class="{ 'animate-in': true }"
          :style="{ animationDelay: `${(index + 6) * 0.1}s` }"
        >
          <h3>{{ benchmark.title }}</h3>
          <p class="number animate-number">{{ benchmark.value }}</p>
          <p class="label">UUIDs/second</p>
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
  name: 'Home',
  components: {
    CodeBlock
  },
  data() {
    return {
      features: [
        {
          icon: 'fas fa-bolt',
          title: 'High Performance',
          description: 'Optimized for speed with zero-allocation operations'
        },
        {
          icon: 'fas fa-shield-alt',
          title: 'Secure by Default',
          description: 'Cryptographically secure random number generation'
        },
        {
          icon: 'fas fa-sync',
          title: 'Thread Safe',
          description: 'Safe for concurrent operations in multi-threaded environments'
        },
        {
          icon: 'fas fa-memory',
          title: 'Memory Efficient',
          description: 'Zero-allocation options for high-throughput scenarios'
        },
        {
          icon: 'fas fa-clock',
          title: 'UUIDv7 Support',
          description: 'Time-ordered, monotonic UUIDs with enhanced uniqueness'
        },
        {
          icon: 'fas fa-rocket',
          title: 'Bulk UUID Generation',
          description: 'Efficiently generate large arrays of UUIDs with minimal overhead'
        }
      ],
      quickStartCode: `// Install via NuGet
dotnet add package UUID

// Generate a new UUID
var id = new UUID();

// Convert to string
string str = id.ToString();

// Parse from string
UUID parsed = UUID.Parse("0123456789abcdef0123456789abcdef");

// Bulk UUID Generation
UUID[] uuids = new UUID[1000];
uuids.Fill();  // Efficiently fills array with new UUIDs

// Generate with error handling
if (UUID.TryGenerate(1000, out UUID[]? result))
{
    // Use the generated UUIDs
    foreach (var uuid in result)
    {
        Console.WriteLine(uuid);
    }
}`,
      benchmarks: [
        { title: 'Single Thread', value: '20.5M' },
        { title: '4 Threads', value: '78.2M' },
        { title: '8 Threads', value: '152.3M' },
        { title: '16 Threads', value: '289.8M' }
      ]
    }
  },
  mounted() {
    Prism.highlightAll()
  }
}
</script>

<style scoped>
.home {
  max-width: 1200px;
  margin: 0 auto;
  padding: 40px 20px;
}

.hero {
  text-align: center;
  margin-bottom: 60px;
}

.hero h1 {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.hero p {
  font-size: 1.2rem;
  color: #a7a7a7;
  margin-bottom: 2rem;
}

.cta-buttons {
  display: flex;
  gap: 20px;
  justify-content: center;
}

.btn {
  display: inline-block;
  padding: 12px 24px;
  border-radius: 6px;
  font-weight: 500;
  text-decoration: none;
  transition: all 0.3s ease;
}

.btn.primary {
  background-color: #ffd700;
  color: #000;
}

.btn.primary:hover {
  background-color: #ffed4a;
}

.btn.secondary {
  background-color: rgba(255, 255, 255, 0.1);
  color: #fff;
}

.btn.secondary:hover {
  background-color: rgba(255, 255, 255, 0.2);
}

section {
  margin-bottom: 60px;
}

section h2 {
  text-align: center;
  margin-bottom: 40px;
  font-size: 2rem;
}

.feature-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 30px;
  margin: 0 auto;
}

.feature-card {
  background-color: #1e1e1e;
  padding: 30px;
  border-radius: 8px;
  text-align: center;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.feature-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
}

.feature-card i {
  font-size: 2rem;
  color: #ffd700;
  margin-bottom: 20px;
  display: inline-block;
}

.feature-card h3 {
  margin-bottom: 15px;
  font-size: 1.2rem;
}

.feature-card p {
  color: #a7a7a7;
  line-height: 1.5;
}

.benchmark-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 30px;
}

.benchmark-card {
  background-color: #1e1e1e;
  padding: 30px;
  border-radius: 8px;
  text-align: center;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  position: relative;
  overflow: hidden;
}

.benchmark-card::before {
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

.benchmark-card:hover::before {
  left: 100%;
}

.benchmark-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(255, 215, 0, 0.1);
}

.benchmark-card .number {
  font-size: 2.5rem;
  font-weight: 700;
  color: #ffd700;
  margin: 15px 0;
  background: linear-gradient(45deg, #ffd700, #ffed4a);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  position: relative;
}

.animate-number {
  animation: countUp 2s ease-out forwards;
  display: inline-block;
}

@keyframes countUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.benchmark-card .label {
  color: #a7a7a7;
  font-size: 1rem;
  position: relative;
}

.benchmark-card .label::after {
  content: '';
  position: absolute;
  bottom: -5px;
  left: 50%;
  width: 0;
  height: 2px;
  background: #ffd700;
  transition: 0.3s ease;
  transform: translateX(-50%);
}

.benchmark-card:hover .label::after {
  width: 50%;
}

.animate-title {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards;
}

.animate-subtitle {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.2s;
}

.animate-buttons {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.4s;
}

.animate-code {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 0.6s ease forwards 0.2s;
}

.animate-icon {
  animation: pulse 2s infinite;
}

.animate-in {
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

@media (max-width: 768px) {
  .hero h1 {
    font-size: 2rem;
  }

  .hero p {
    font-size: 1rem;
  }

  .cta-buttons {
    flex-direction: column;
    gap: 10px;
  }

  .btn {
    width: 100%;
    text-align: center;
  }

  .feature-grid {
    grid-template-columns: 1fr;
  }

  .benchmark-grid {
    grid-template-columns: 1fr 1fr;
  }
}

@media (max-width: 480px) {
  .cta-buttons {
    flex-direction: column;
    gap: 10px;
  }

  .benchmark-grid {
    grid-template-columns: 1fr;
  }
}
</style>
