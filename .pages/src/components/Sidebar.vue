<template>
  <div class="sidebar" :class="{ 'mobile-open': isMobileOpen }">
    <div class="logo-section">
      <img src="../assets/img/Logo.svg" alt="UUID Logo" class="logo-img">
      <h1>UUID</h1>
    </div>
    
    <nav class="nav-section">
      <ul>
        <li>
          <router-link to="/" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-home"></i>
            <span>Home</span>
          </router-link>
        </li>
        <li>
          <router-link to="/getting-started" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-rocket"></i>
            <span>Getting Started</span>
          </router-link>
        </li>
        <li>
          <router-link to="/api-reference" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-book"></i>
            <span>API Reference</span>
          </router-link>
        </li>
        <li>
          <router-link to="/examples" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-code"></i>
            <span>Examples</span>
          </router-link>
        </li>
        <li>
          <router-link to="/serialization" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-random"></i>
            <span>Serialization</span>
          </router-link>
        </li>
        <li>
          <router-link to="/performance" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-tachometer-alt"></i>
            <span>Performance</span>
          </router-link>
        </li>
        <li>
          <router-link to="/security" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-shield-alt"></i>
            <span>Security</span>
          </router-link>
        </li>
        <li>
          <router-link to="/migration" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-exchange-alt"></i>
            <span>Migration</span>
          </router-link>
        </li>
        <li>
          <router-link to="/faq" class="nav-link" @click="closeMobileMenu">
            <i class="fas fa-question-circle"></i>
            <span>FAQ</span>
          </router-link>
        </li>
      </ul>
    </nav>

    <div class="social-links">
      <a href="https://github.com/Taiizor/UUID" target="_blank" rel="noopener">
        <i class="fab fa-github"></i>
      </a>
      <a href="https://discord.gg/nxG977byXb" target="_blank" rel="noopener">
        <i class="fab fa-discord"></i>
      </a>
      <a href="https://stackoverflow.com/questions/tagged/uuid" target="_blank" rel="noopener">
        <i class="fab fa-stack-overflow"></i>
      </a>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Sidebar',
  props: {
    isMobileOpen: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    closeMobileMenu() {
      if (window.innerWidth <= 768) {
        this.$emit('update:isMobileOpen', false)
      }
    }
  }
}
</script>

<style scoped>
.sidebar {
  width: 280px;
  height: 100vh;
  background-color: #1e1e1e;
  color: #fff;
  position: fixed;
  left: 0;
  top: 0;
  display: flex;
  flex-direction: column;
  z-index: 1000;
  transition: transform 0.3s ease;
  overflow-y: auto;
  -webkit-overflow-scrolling: touch;
}

@media (max-width: 768px) {
  .sidebar {
    transform: translateX(-100%);
    box-shadow: none;
    height: 100%;
    min-height: -webkit-fill-available;
    max-height: 100vh;
  }

  .sidebar.mobile-open {
    transform: translateX(0);
    box-shadow: 2px 0 10px rgba(0, 0, 0, 0.3);
  }

  .nav-section {
    flex: 1;
    overflow-y: auto;
  }

  .social-links {
    padding: 20px;
    background-color: #1e1e1e;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
  }
}

.logo-section {
  padding: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
}

.logo-img {
  width: 32px;
  height: 32px;
}

.logo-section h1 {
  color: #fff;
  font-size: 24px;
  margin: 0;
}

.nav-section {
  flex: 1;
  padding: 12px 0;
  overflow-y: auto;
  width: 100%;
}

nav ul {
  list-style: none;
  padding: 0;
  margin: 0;
  width: 100%;
}

nav ul li {
  margin: 2px 0;
  width: 100%;
}

.nav-link {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  color: #a7a7a7;
  text-decoration: none;
  transition: all 0.2s ease;
  gap: 12px;
  margin: 4px;
  border-radius: 6px;
  width: calc(100% - 8px);
  min-height: 44px;
}

.nav-link:hover {
  color: #fff;
  background-color: rgba(255, 255, 255, 0.1);
}

.nav-link.router-link-active {
  color: #000;
  background-color: #ffd700;
  font-weight: 500;
}

.nav-link i {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 20px;
  height: 20px;
  font-size: 16px;
  flex-shrink: 0;
  line-height: 1;
}

.nav-link span {
  font-size: 14px;
  font-weight: 400;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  line-height: 1.4;
  padding: 2px 0;
}

.nav-link.router-link-active i {
  color: #000;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.nav-link.router-link-active span {
  color: #000;
  font-weight: 500;
}

.social-links {
  padding: 20px;
  display: flex;
  justify-content: center;
  gap: 20px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  background-color: #1e1e1e;
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
}

.social-links a {
  color: #a7a7a7;
  font-size: 20px;
  transition: color 0.3s ease;
}

.social-links a:hover {
  color: #fff;
}
</style>
