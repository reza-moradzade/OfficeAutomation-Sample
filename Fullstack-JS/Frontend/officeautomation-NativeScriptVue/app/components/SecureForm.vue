<template>
  <Page>
    <!-- ActionBar at the top with page title -->
    <ActionBar title="Secure Form" />

    <StackLayout>
      <!-- Display message if user is not authenticated -->
      <Label v-if="!authOK" text="You are not authorized. Please login." />

      <!-- Show secure form content only if authenticated -->
      <StackLayout v-else>
        <!-- Actual secure form fields -->
        <Label text="This is a secure form." />
      </StackLayout>
    </StackLayout>
  </Page>
</template>

<script>
import auth from "../services/auth";

export default {
  data() {
    return {
      authOK: false // Tracks whether the user is authenticated
    };
  },
  created() {
    // Check authentication status on component creation
    this.authOK = auth.isAuthenticated();

    if (!this.authOK) {
      // Optional: redirect to Home or show a toast
      // Note: Be careful with circular imports if navigating here
      // this.$navigateTo(require('./Home').default)
    }
  }
};
</script>
