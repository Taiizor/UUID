<template>
  <div class="code-block-wrapper">
    <div class="code-block">
      <button class="copy-button" @click="copyCode" :class="{ copied: isCopied }" :title="isCopied ? 'Copied!' : 'Copy code'">
        <i :class="isCopied ? 'fas fa-check' : 'fas fa-copy'"></i>
      </button>
      <pre><code :class="language"><slot></slot></code></pre>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CodeBlock',
  props: {
    language: {
      type: String,
      default: 'language-csharp'
    }
  },
  data() {
    return {
      isCopied: false
    }
  },
  methods: {
    async copyCode() {
      const code = this.$el.querySelector('code').textContent
      try {
        await navigator.clipboard.writeText(code)
        this.isCopied = true
        setTimeout(() => {
          this.isCopied = false
        }, 2000)
      } catch (err) {
        console.error('Failed to copy code:', err)
      }
    }
  }
}
</script>

<style scoped>
.code-block-wrapper {
  position: relative;
  margin: 1rem 0;
}

.code-block {
  background-color: #1e1e1e;
  border-radius: 8px;
  padding: 20px;
  overflow: hidden;
  position: relative;
}

.code-block:hover .copy-button {
  opacity: 1;
  transform: translateX(0);
}

.copy-button {
  position: absolute;
  top: 12px;
  right: 12px;
  background: rgba(255, 255, 255, 0.1);
  border: none;
  border-radius: 4px;
  color: #a7a7a7;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.3s ease;
  opacity: 0;
  transform: translateX(10px);
  backdrop-filter: blur(4px);
}

.copy-button:hover {
  background: rgba(255, 255, 255, 0.2);
  color: #fff;
}

.copy-button.copied {
  background: #4caf50;
  color: #fff;
  opacity: 1;
}

.copy-button i {
  font-size: 0.9rem;
}

.code-block pre {
  margin: 0;
  padding-right: 40px;
}

.code-block code {
  font-family: 'Consolas', 'Monaco', monospace;
  color: #d4d4d4;
  display: block;
}

@media (max-width: 768px) {
  .code-block {
    padding: 15px;
  }

  .copy-button {
    opacity: 1;
    transform: translateX(0);
  }
}
</style>