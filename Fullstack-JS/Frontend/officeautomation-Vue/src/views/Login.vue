<template>
  <!-- Main container: centers the login card both vertically and horizontally -->
  <div class="d-flex justify-content-center align-items-center flex-grow-1 bg-light" style="min-height: 100vh;">
    
    <!-- Login card with padding and shadow -->
    <div class="card p-4 shadow" style="width: 100%; max-width: 400px;">
      
      <!-- Card title -->
      <h2 class="card-title text-center mb-4">Login</h2>
      
      <!-- Login form with preventDefault on submit -->
      <form @submit.prevent="login">
        
        <!-- Username input field -->
        <div class="mb-3">
          <label for="username" class="form-label">Username</label>
          <input 
            id="username" 
            v-model="username" 
            type="text" 
            class="form-control" 
            placeholder="Enter username" 
            required 
          />
        </div>
        
        <!-- Password input field -->
        <div class="mb-3">
          <label for="password" class="form-label">Password</label>
          <input 
            id="password" 
            v-model="password" 
            type="password" 
            class="form-control" 
            placeholder="Enter password" 
            required 
          />
        </div>
        
        <!-- Submit button -->
        <button type="submit" class="btn btn-primary w-100">Login</button>
        
        <!-- Error message display -->
        <p v-if="error" class="text-danger mt-2 text-center">{{ error }}</p>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { useRouter } from 'vue-router';
import axios from 'axios';

export default defineComponent({
  setup() {
    // Reactive references for form data
    const username = ref(''); // Stores the entered username
    const password = ref(''); // Stores the entered password
    const error = ref('');    // Stores error messages for display

    const router = useRouter(); // Vue Router instance for navigation

    /**
     * Handles user login
     * Sends POST request to API with username and password
     * On success: stores JWT token and navigates to home
     * On failure: shows error message
     */
    const login = async () => {
      try {
        const res = await axios.post('http://localhost:3000/api/auth/login', {
          username: username.value,
          password: password.value
        });

        // Store JWT token in localStorage
        localStorage.setItem('token', res.data.token);

        // Navigate to home page after successful login
        router.push('/home');
      } catch (err: any) {
        // Display error from API response or default message
        error.value = err.response?.data?.message || 'Login failed';
      }
    };

    return { username, password, error, login };
  }
});
</script>

<style scoped>
/* Optional styling for better appearance */
body {
  margin: 0; /* Remove default body margin */
}
</style>
