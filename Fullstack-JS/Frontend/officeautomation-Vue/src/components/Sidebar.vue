<template>
  <!-- Sidebar container: vertical navigation menu -->
  <div class="sidebar d-flex flex-column p-3 bg-light shadow" style="width: 220px; min-height: 100vh;">
    
    <!-- Optional: App title (commented out) -->
    <!-- <h5 class="mb-4 text-center">Office Automation</h5> -->
    
    <!-- Navigation list using Bootstrap pills -->
    <ul class="nav nav-pills flex-column mb-auto">
      
      <!-- Home navigation link -->
      <li class="nav-item mb-2">
        <router-link 
          to="/home" 
          class="nav-link text-dark" 
          active-class="active"
        >
          Home
        </router-link>
      </li>
      
      <!-- Cartable navigation link -->
      <li class="nav-item mb-2">
        <router-link 
          to="/cartable" 
          class="nav-link text-dark" 
          active-class="active"
        >
          Cartable
        </router-link>
      </li>
      
      <!-- Logout link positioned at the bottom -->
      <li class="nav-item mt-auto">
        <a 
          href="#" 
          @click.prevent="logout" 
          class="nav-link text-danger"
        >
          Logout
        </a>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'Sidebar', // Component name for devtools and debugging
  setup() {
    const router = useRouter(); // Vue Router instance for navigation

    /**
     * Handles user logout
     * Removes JWT token from localStorage
     * Redirects user to Login page
     */
    const logout = () => {
      localStorage.removeItem('token'); // Remove stored authentication token
      router.push('/');                  // Navigate to Login page
    };

    return { logout };
  }
});
</script>

<style scoped>
/* Sidebar container styling */
.sidebar {
  transition: all 0.3s ease;
  border-right: 1px solid #dee2e6;
}

/* Navigation link styling */
.nav-link {
  border-radius: 0.375rem;
  transition: background 0.2s;
}

/* Hover effect for nav links */
.nav-link:hover {
  background-color: #e2e6ea;
}

/* Active link styling */
.nav-link.active {
  background-color: #0d6efd;
  color: #fff !important;
}
</style>
