<template>
  <div class="error-page">
    <div class="error-container">
      <div class="error-icon">
        <i class="fas fa-server pulse"></i>
      </div>
      <h1 class="glow">500</h1>
      <h2>Server Error</h2>
      <p>Sorry, something went wrong on our end. Please try again later.</p>
      <router-link to="/" class="home-button">
        <i class="fas fa-home"></i> Back to Home
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ServerError'
}
</script>

<style scoped>
.error-page {
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: var(--bg-primary);
  padding: 2rem;
  overflow: hidden;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
}

.error-container {
  text-align: center;
  padding: 2rem;
  background-color: var(--bg-secondary);
  border-radius: 8px;
  box-shadow: 0 0 20px rgba(255, 215, 0, 0.1);
  max-width: 600px;
  width: 90%;
  animation: fadeIn 0.5s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.error-icon {
  font-size: 4rem;
  color: #ffd700;
  margin-bottom: 1rem;
}

.pulse {
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

h1 {
  font-size: 6rem;
  color: #ffd700;
  margin: 0;
  line-height: 1;
  font-weight: 700;
}

.glow {
  animation: glow 2s ease-in-out infinite alternate;
}

@keyframes glow {
  from {
    text-shadow: 0 0 5px #fff, 0 0 10px #fff, 0 0 15px #ffd700, 0 0 20px #ffd700;
  }
  to {
    text-shadow: 0 0 10px #fff, 0 0 20px #fff, 0 0 30px #ffd700, 0 0 40px #ffd700;
  }
}

h2 {
  font-size: 2rem;
  color: var(--text-primary);
  margin: 1rem 0;
  font-weight: 600;
}

p {
  color: var(--text-secondary);
  margin-bottom: 2rem;
  font-size: 1.1rem;
}

.home-button {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.8rem 1.5rem;
  background-color: #ffd700;
  color: var(--bg-primary);
  text-decoration: none;
  border-radius: 5px;
  transition: all 0.3s ease;
  font-weight: 500;
  position: relative;
  overflow: hidden;
}

.home-button:hover {
  background-color: #ffed4a;
  transform: translateY(-2px);
}

.home-button::after {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: rgba(255, 255, 255, 0.1);
  transform: rotate(45deg);
  transition: all 0.3s ease;
}

.home-button:hover::after {
  transform: rotate(45deg) translate(50%, 50%);
}

.home-button i {
  font-size: 1.1rem;
}

@media (max-width: 768px) {
  .error-container {
    padding: 2rem;
    margin: 1rem;
  }

  h1 {
    font-size: 4rem;
  }

  h2 {
    font-size: 1.5rem;
  }
}
</style>