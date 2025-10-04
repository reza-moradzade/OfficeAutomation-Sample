<template>
  <div class="main-content">
    <div class="content-area">
      <component :is="currentComponent" />
    </div>
  </div>
</template>

<script setup>
// Import page components
import HomePage from './HomePage.vue'
import CartablePage from './CartablePage.vue'

// Define available page components
const components = {
  home: HomePage,
  cartable: CartablePage
}

// Props from parent component
const props = defineProps({
  activeMenu: {
    type: String,
    default: 'home'
  }
})

// Current component based on selected menu
const currentComponent = computed(() => {
  return components[props.activeMenu] || HomePage
})
</script>

<style scoped>
.main-content {
  flex: 1;
  background: #f8f9fa;
  height: 100vh;
  overflow-y: auto;
}

.content-area {
  padding: 2rem;
  height: 100%;
}
</style>