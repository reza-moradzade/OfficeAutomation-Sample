import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Login.vue';
import Home from '../views/Home.vue';
import Cartable from '../views/Cartable.vue';

// Define application routes
const routes = [
  { 
    path: '/login', 
    component: Login // Route for Login page only
  },
  { 
    path: '/home', 
    component: Home // Route for Home page
  },
  { 
    path: '/cartable', 
    component: Cartable // Route for Cartable page
  },
  { 
    path: '/', 
    redirect: '/login' // Default redirect to Login page
  }
];

// Create Vue Router instance with HTML5 history mode
const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router; // Export router to be used in main.ts
