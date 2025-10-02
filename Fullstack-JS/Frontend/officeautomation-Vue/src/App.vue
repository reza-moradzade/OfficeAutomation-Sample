<template>
  <!-- Root app container with full viewport height -->
  <div id="app" class="d-flex flex-column min-vh-100">

    <!-- Header section -->
    <header class="bg-primary text-white py-3 shadow">
      <div class="container d-flex justify-content-between align-items-center">
        
        <!-- App title, left-aligned -->
        <h4 class="mb-0 text-start">Office Automation</h4>
        
        <!-- Display user name only if sidebar is visible -->
        <div v-if="showSidebar">
          <span>Reza Moradzade</span>
        </div>
      </div>
    </header>

    <!-- Main content area -->
    <div class="d-flex flex-grow-1">
      
      <!-- Sidebar component, shown on all pages except Login -->
      <Sidebar v-if="showSidebar" />

      <!-- Main content for router views (Login, Home, Cartable, etc.) -->
      <main class="flex-grow-1 p-4">
        <router-view />
      </main>
    </div>

    <!-- Footer section -->
    <footer class="bg-light text-center py-3 mt-auto border-top">
      &copy; 2025 Office Automation. All rights reserved.
    </footer>
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue';
import Sidebar from './components/Sidebar.vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  components: { Sidebar }, // Register Sidebar component
  setup() {
    const router = useRouter();

    /**
     * Determines whether to show the sidebar
     * Sidebar is hidden on the Login page
     */
    const showSidebar = computed(() => router.currentRoute.value.path !== '/login');

    return { showSidebar };
  }
});
</script>

<style scoped>
/* Main content background for all pages except header/footer */
#app > .d-flex.flex-grow-1 {
  background: #f8f9fa;
}

/* Left-align header title */
header h4 {
  text-align: left;
}
</style>
