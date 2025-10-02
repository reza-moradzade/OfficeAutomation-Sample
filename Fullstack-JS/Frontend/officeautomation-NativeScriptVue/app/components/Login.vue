<template>
  <StackLayout spacing="10" width="100%">
    
    <!-- Username input field -->
    <TextField
      v-model="username"
      hint="Username"
      autocorrect="false"
      autocapitalizationType="none"
    />

    <!-- Password input field -->
    <TextField
      v-model="password"
      hint="Password"
      secure="true"
    />

    <!-- Login button (کامنت برای این بخش حذف شد) -->
    <Button
      :text="loading ? 'Logging...' : 'Login'"
      :isEnabled="!loading"
      @tap="onLogin"
    />

    <!-- Label to show success or error messages -->
    <Label :text="message" class="text-center mt-10" textWrap="true" />
  </StackLayout>
</template>

<script>
import auth from "../services/auth";

export default {
  data() {
    return {
      username: "reza",      // مقدار پیش‌فرض
      password: "123456",    // مقدار پیش‌فرض
      message: "",           // پیام وضعیت لاگین
      loading: false,        // وضعیت بارگذاری دکمه لاگین
    };
  },
  methods: {
    /**
     * Handles login logic:
     * Calls authentication service, saves token, and emits event on success
     */
    async onLogin() {
      this.message = "";       // Reset message
      this.loading = true;     // Set loading state
      try {
        // Call login API simulation
        const res = await auth.loginRequest(this.username, this.password);

        // Save token locally
        auth.saveToken(res.token);

        // Show success message and notify parent component
        this.message = "✅ Login successful!";
        this.$emit("login-success");
      } catch (err) {
        // Show error message on login failure
        this.message = "❌ Wrong username or password!";
      } finally {
        this.loading = false; // Reset loading state
      }
    },
  },
};
</script>

<style scoped>
/* Center text for messages */
.text-center { text-align: center; color: #1976d2; }

/* Top margin utility */
.mt-10 { margin-top: 10; }
</style>
