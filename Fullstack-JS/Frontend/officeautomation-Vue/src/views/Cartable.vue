<template>
  <!-- Main container for the cartable page -->
  <div class="container py-4">
    
    <!-- Page title -->
    <h2 class="mb-4">Cartable</h2>
    
    <!-- Grid row for tasks -->
    <div class="row g-3">
      
      <!-- Task card, iterates over cartable array -->
      <div class="col-md-4" v-for="task in cartable" :key="task.cartableid">
        <div class="card h-100 shadow-sm">
          <div class="card-body">
            
            <!-- Task title showing task ID -->
            <h5 class="card-title">Task #{{ task.taskid }}</h5>
            
            <!-- Task status: changes color based on read/unread -->
            <p class="card-text">
              Status: 
              <span :class="task.isread ? 'text-success' : 'text-danger'">
                {{ task.isread ? 'Read' : 'Unread' }}
              </span>
            </p>
          </div>
        </div>
      </div>
      
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import axios from 'axios';

export default defineComponent({
  setup() {
    const cartable = ref<any[]>([]); // Reactive array to store tasks

    // Fetch cartable tasks on component mount
    onMounted(async () => {
      const token = localStorage.getItem('token'); // Get JWT token from localStorage
      if (!token) return; // Exit if token is missing

      try {
        // Call API to fetch cartable tasks with Authorization header
        const res = await axios.get('http://localhost:3000/api/cartable', {
          headers: { Authorization: `Bearer ${token}` }
        });

        // Update reactive cartable array with API response
        cartable.value = res.data.cartable;
      } catch (err) {
        // Log any API errors to console
        console.error(err);
      }
    });

    return { cartable };
  }
});
</script>

<style scoped>
/* Card styling with smooth hover effects */
.card {
  border-radius: 8px;
  transition: transform 0.2s, box-shadow 0.2s;
}

/* Hover effect: lift the card and add shadow */
.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15);
}
</style>
