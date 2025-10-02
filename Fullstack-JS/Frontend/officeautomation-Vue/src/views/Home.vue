<template>
  <!-- Main container: centers the welcome card vertically and horizontally -->
  <div class="d-flex justify-content-center align-items-center flex-grow-1" style="min-height: 100vh; background: #f8f9fa;">
    
    <!-- Welcome card with padding, shadow, and centered text -->
    <div class="card p-4 shadow text-center" style="width: 100%; max-width: 500px;">
      
      <!-- Card title -->
      <h2 class="card-title mb-3">Welcome</h2>
      
      <!-- Card text content -->
      <p class="card-text">You are successfully logged in.</p>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Home' // Component name for debugging and devtools
});
</script>

<style scoped>
/* Optional: Add gradient background or additional styling if needed */
</style>
