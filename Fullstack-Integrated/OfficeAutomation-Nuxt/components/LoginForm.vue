<template>
  <div class="login-container">
    <form @submit.prevent="handleLogin" class="login-form">
      <h2>Office Automation System</h2>
      
      <div class="form-group">
        <label for="email">Email:</label>
        <input
          id="email"
          v-model="form.email"
          type="email"
          placeholder="admin@office.com"
          required
          class="form-input"
        />
      </div>

      <div class="form-group">
        <label for="password">Password:</label>
        <input
          id="password"
          v-model="form.password"
          type="password"
          placeholder="Enter your password"
          required
          class="form-input"
        />
      </div>

      <button 
        type="submit" 
        :disabled="loading" 
        class="login-button"
        :class="{ 'loading': loading }"
      >
        {{ loading ? 'Signing in...' : 'Sign In' }}
      </button>

      <div v-if="error" class="error-message">
        {{ error }}
      </div>

      <div class="demo-info">
        <p><strong>Demo Credentials:</strong></p>
        <p>Email: admin@office.com</p>
        <p>Password: 123456</p>
      </div>
    </form>
  </div>
</template>

<script setup>
// Reactive form state
const form = reactive({
  email: '',
  password: ''
})

// Component states
const loading = ref(false)
const error = ref('')

// Login handler function
const handleLogin = async () => {
  // Reset error state
  error.value = ''
  
  // Simple validation
  if (!form.email || !form.password) {
    error.value = 'Please fill in all fields'
    return
  }

  // Simulate login process
  loading.value = true
  
  try {
    // API call to backend - Correct usage of $fetch
    const response = await $fetch('/api/auth/login', {
      method: 'POST',
      body: JSON.stringify(form),
      headers: {
        'Content-Type': 'application/json'
      }
    })
    
    console.log('Login response:', response)
    
    if (response.success) {
      console.log('Login successful - Redirecting to dashboard...')
      
      // Redirect to main dashboard
      await navigateTo('/dashboard')
    } else {
      error.value = response.message || 'Login failed'
    }
    
  } catch (err) {
    console.error('Login error details:', err)
    error.value = err.data?.statusMessage || err.message || 'Login error. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
/* Styles remain the same */
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.login-form {
  background: white;
  padding: 2rem;
  border-radius: 10px;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
  width: 100%;
  max-width: 400px;
  direction: ltr;
}

h2 {
  text-align: center;
  margin-bottom: 1.5rem;
  color: #333;
}

.form-group {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  color: #555;
  font-weight: 500;
}

.form-input {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e1e5e9;
  border-radius: 5px;
  font-size: 1rem;
  transition: border-color 0.3s ease;
  box-sizing: border-box;
}

.form-input:focus {
  outline: none;
  border-color: #667eea;
}

.login-button {
  width: 100%;
  padding: 0.75rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: opacity 0.3s ease;
  margin-bottom: 1rem;
}

.login-button:hover:not(:disabled) {
  opacity: 0.9;
}

.login-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.login-button.loading {
  position: relative;
  color: transparent;
}

.login-button.loading::after {
  content: '';
  position: absolute;
  width: 20px;
  height: 20px;
  top: 50%;
  left: 50%;
  margin-left: -10px;
  margin-top: -10px;
  border: 2px solid transparent;
  border-top: 2px solid white;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  background: #fee;
  color: #c33;
  padding: 0.75rem;
  border-radius: 5px;
  margin-bottom: 1rem;
  text-align: center;
  border: 1px solid #fcc;
}

.demo-info {
  background: #e3f2fd;
  border: 1px solid #90caf9;
  border-radius: 8px;
  padding: 1rem;
  text-align: center;
  font-size: 0.9rem;
  color: #1565c0;
}

.demo-info p {
  margin: 0.25rem 0;
}
</style>