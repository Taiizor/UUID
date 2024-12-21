import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/home.vue'

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
    component: () => import('../views/gettingstarted.vue'),
    meta: {
      title: 'Getting Started - UUID Documentation'
    }
  },
  {
    path: '/api-reference',
    name: 'APIReference',
    component: () => import('../views/apireference.vue'),
    meta: {
      title: 'API Reference - UUID Documentation'
    }
  },
  {
    path: '/examples',
    name: 'Examples',
    component: () => import('../views/examples.vue'),
    meta: {
      title: 'Examples - UUID Documentation'
    }
  },
  {
    path: '/serialization',
    name: 'Serialization',
    component: () => import('../views/serialization.vue'),
    meta: {
      title: 'Serialization - UUID Documentation'
    }
  },
  {
    path: '/performance',
    name: 'Performance',
    component: () => import('../views/performance.vue'),
    meta: {
      title: 'Performance - UUID Documentation'
    }
  },
  {
    path: '/security',
    name: 'Security',
    component: () => import('../views/security.vue'),
    meta: {
      title: 'Security - UUID Documentation'
    }
  },
  {
    path: '/migration',
    name: 'Migration',
    component: () => import('../views/migration.vue'),
    meta: {
      title: 'Migration Guide - UUID Documentation'
    }
  },
  {
    path: '/faq',
    name: 'FAQ',
    component: () => import('../views/faq.vue'),
    meta: {
      title: 'FAQ - UUID Documentation'
    }
  },
  {
    path: '/404',
    name: 'NotFound',
    component: () => import('../views/errors/404.vue'),
    meta: {
      title: 'Page Not Found - UUID Documentation'
    }
  },
  {
    path: '/500',
    name: 'ServerError',
    component: () => import('../views/errors/500.vue'),
    meta: {
      title: 'Server Error - UUID Documentation'
    }
  },
  {
    path: '/:pathMatch(.*)*',
    redirect: '/404'
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