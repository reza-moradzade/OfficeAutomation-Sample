<template>
  <Page>
    <!-- Top ActionBar with app title -->
    <ActionBar title="Office Automation" />

    <!-- Main content container, centered vertically and horizontally -->
    <StackLayout class="p-20" verticalAlignment="center" horizontalAlignment="center" spacing="20">

      <!-- Show Login form if user is not authenticated -->
      <Login v-if="!isLoggedIn" @login-success="onLoginSuccess" />

      <!-- Welcome message and logout button after successful login -->
      <StackLayout v-else>
        <Label text="Welcome yasna!" class="h1 text-center" />
        <Label text="This is the Office Automation app." class="h2 text-center text-primary mt-10" />
        <Button text="Logout" class="mt-20" @tap="logout" />
      </StackLayout>

    </StackLayout>
  </Page>
</template>

<script>
import Login from "./Login.vue";
import auth from "../services/auth";

export default {
  name: "Home", // Component name for devtools
  components: { Login },
  data() {
    return {
      // Track whether the user is logged in
      isLoggedIn: auth.isAuthenticated(),
    };
  },
  methods: {
    /**
     * Called when child Login component emits a successful login
     * Updates the isLoggedIn state to show the welcome content
     */
    onLoginSuccess() {
      this.isLoggedIn = true;
    },
    /**
     * Logout user by clearing token and updating state
     */
    logout() {
      auth.clearToken();  // Remove stored authentication token
      this.isLoggedIn = false; // Reset login state
    },
  },
};
</script>

<style scoped>
/* Padding for main container */
.p-20 { padding: 20; }

/* Center text */
.text-center { text-align: center; }

/* Primary text color */
.text-primary { color: #1976d2; }

/* Top margin utilities */
.mt-10 { margin-top: 10; }
.mt-20 { margin-top: 20; }

/* Heading styles */
.h1 { font-size: 24; font-weight: bold; }
.h2 { font-size: 18; }
</style>
