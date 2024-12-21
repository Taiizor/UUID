import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      title: 'UUID - Modern .NET UUID Implementation'
    }
  },
  {
    path: '/getting-started',
    name: 'GettingStarted',
    component: () => import('../views/GettingStarted.vue'),
    meta: {
      title: 'Getting Started - UUID Documentation'
    }
  },
  {
    path: '/api-reference',
    name: 'ApiReference',
    component: () => import('../views/ApiReference.vue'),
    meta: {
      title: 'API Reference - UUID Documentation'
    }
  },
  {
    path: '/examples',
    name: 'Examples',
    component: () => import('../views/Examples.vue'),
    meta: {
      title: 'Examples - UUID Documentation'
    }
  },
  {
    path: '/serialization',
    name: 'Serialization',
    component: () => import('../views/Serialization.vue'),
    meta: {
      title: 'Serialization - UUID Documentation'
    }
  },
  {
    path: '/performance',
    name: 'Performance',
    component: () => import('../views/Performance.vue'),
    meta: {
      title: 'Performance - UUID Documentation'
    }
  },
  {
    path: '/security',
    name: 'Security',
    component: () => import('../views/Security.vue'),
    meta: {
      title: 'Security - UUID Documentation'
    }
  },
  {
    path: '/migration',
    name: 'Migration',
    component: () => import('../views/Migration.vue'),
    meta: {
      title: 'Migration Guide - UUID Documentation'
    }
  },
  {
    path: '/faq',
    name: 'FAQ',
    component: () => import('../views/FAQ.vue'),
    meta: {
      title: 'FAQ - UUID Documentation'
    }
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

router.beforeEach((to, from, next) => {
  document.title = to.meta.title || 'UUID Documentation'
  next()
})

export default router