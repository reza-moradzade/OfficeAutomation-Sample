<template>
  <div>
    <nav class="navbar" v-if="!$route.path.includes('/dashboard')">
      <div class="nav-container">
        <NuxtLink to="/" class="logo">Office Automation</NuxtLink>
        <div class="nav-links">
          <NuxtLink to="/login" class="nav-link">Login</NuxtLink>
        </div>
      </div>
    </nav>
    
    <NuxtPage />
  </div>
</template>

<script setup>
// Main app configuration
// No need to initialize database here, it's done in server plugin
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  line-height: 1.6;
  color: #333;
}

.navbar {
  background: #fff;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  padding: 1rem 0;
}

.nav-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.logo {
  font-size: 1.5rem;
  font-weight: bold;
  color: #667eea;
  text-decoration: none;
}

.nav-links {
  display: flex;
  gap: 1rem;
}

.nav-link {
  color: #333;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 5px;
  transition: background-color 0.3s ease;
}

.nav-link:hover {
  background-color: #f5f5f5;
}
</style>