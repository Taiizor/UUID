<template>
  <div class="app">
    <div 
      v-if="isMobileOpen" 
      class="sidebar-overlay"
      @click="closeSidebar"
    ></div>
    <Sidebar :is-mobile-open="isMobileOpen" />
    <main class="main-content">
      <router-view></router-view>
    </main>
    <button 
      class="sidebar-toggle"
      @click="toggleSidebar"
      :class="{ 'active': isMobileOpen }"
    >
      <i class="fas" :class="isMobileOpen ? 'fa-times' : 'fa-bars'"></i>
    </button>
  </div>
</template>

<script>
import Sidebar from './components/Sidebar.vue'

export default {
  name: 'App',
  components: {
    Sidebar
  },
  data() {
    return {
      isMobileOpen: false
    }
  },
  watch: {
    $route() {
      this.scrollToTop()
    }
  },
  methods: {
    toggleSidebar() {
      this.isMobileOpen = !this.isMobileOpen
    },
    closeSidebar() {
      this.isMobileOpen = false
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      })
      // Ana içerik alanını da sıfırla
      const mainContent = document.querySelector('.main-content')
      if (mainContent) {
        mainContent.scrollTo({
          top: 0,
          behavior: 'smooth'
        })
      }
    }
  },
  mounted() {
    // Sayfa ilk yüklendiğinde de scroll'u sıfırla
    this.scrollToTop()
  }
}
</script>

<style>
@import '@fortawesome/fontawesome-free/css/all.css';
@import './assets/css/style.css';

/* Webkit (Chrome, Safari, Edge) için scroll bar stili */
::-webkit-scrollbar {
  width: 10px;
  height: 10px;
}

::-webkit-scrollbar-track {
  background: #1a1a1a;
  border-radius: 5px;
}

::-webkit-scrollbar-thumb {
  background: #333;
  border-radius: 5px;
  border: 2px solid #1a1a1a;
  transition: all 0.3s ease;
}

::-webkit-scrollbar-thumb:hover {
  background: #ffd700;
}

/* Firefox için scroll bar stili */
* {
  scrollbar-width: thin;
  scrollbar-color: #333 #1a1a1a;
}

html, body {
  margin: 0;
  padding: 0;
  height: 100%;
}

body {
  min-height: 100vh;
  min-height: -webkit-fill-available;
}

.app {
  display: flex;
  min-height: 100vh;
  position: relative;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 40px;
  background-color: #121212;
  color: #fff;
  min-height: 100vh;
  overflow-y: auto;
  -webkit-overflow-scrolling: touch;
}

.sidebar-toggle {
  display: none;
  position: fixed;
  bottom: 25px;
  right: 25px;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #ffd700;
  border: none;
  color: #1a1a1a;
  font-size: 1.3rem;
  cursor: pointer;
  z-index: 1000;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3);
  transition: all 0.3s ease;
}

.sidebar-toggle:hover {
  transform: scale(1.1);
  background: #ffe44d;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.4);
}

.sidebar-toggle.active {
  background: #ff4444;
  color: #fff;
}

.sidebar-toggle.active:hover {
  background: #ff6666;
}

.sidebar-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  z-index: 999;
  transition: opacity 0.3s ease;
}

@media (max-width: 768px) {
  .main-content {
    margin-left: 0;
    padding: 20px;
    width: 100%;
    height: 100vh;
    overflow-y: auto;
  }

  .sidebar-toggle {
    display: flex;
    align-items: center;
    justify-content: center;
  }

  html {
    height: -webkit-fill-available;
  }

  body {
    height: 100vh;
    overflow-y: auto;
  }

  .app {
    overflow-x: hidden;
    overflow-y: auto;
    height: auto;
    min-height: 100vh;
  }

  .sidebar-overlay {
    position: fixed;
    overflow-y: auto;
  }
}

h1, h2, h3, h4, h5, h6 {
  color: #fff;
  margin-bottom: 1rem;
}

a {
  color: #ffd700;
  text-decoration: none;
}

a:hover {
  text-decoration: underline;
}

.code-block {
  background-color: #1e1e1e;
  border-radius: 4px;
  padding: 1rem;
  margin: 1rem 0;
}

pre {
  margin: 0;
}

code {
  font-family: 'Consolas', 'Monaco', monospace;
  color: #fff;
}
</style>