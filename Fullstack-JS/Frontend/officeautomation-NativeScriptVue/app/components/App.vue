<template>
  <Page>
    <!-- Top ActionBar with app title -->
    <ActionBar title="Office Automation" />

    <!-- Main container, centered vertically and horizontally with spacing -->
    <StackLayout class="p-20" verticalAlignment="center" horizontalAlignment="center" spacing="20">

      <!-- Show Login form only if user is not authenticated -->
      <Login v-if="!isLoggedIn" @login-success="onLoginSuccess" />

      <!-- Show TabView with main app content after login -->
      <TabView v-else androidTabsPosition="bottom">
        
        <!-- Home tab -->
        <TabViewItem title="🏠 Home">
          <Frame id="items">
            <Items @logout="handleLogout"/> <!-- Pass logout handler to Items component -->
          </Frame>
        </TabViewItem>

        <!-- Cartable tab -->
        <TabViewItem title="📂 Cartable">
          <Frame id="cartable">
            <Browse/> <!-- Cartable content -->
          </Frame>
        </TabViewItem>

        <!-- Search tab -->
        <TabViewItem title="🔍 Search">
          <Frame id="search">
            <Search/> <!-- Search content -->
          </Frame>
        </TabViewItem>

      </TabView>

    </StackLayout>
  </Page>
</template>

<script>
import Login from "./Login.vue";
import Items from "./Items.vue";
import Browse from "./Cartable.vue";
import Search from "./Search.vue";
import auth from "../services/auth";

export default {
  components: { Login, Items, Browse, Search },
  data() {
    return {
      // Tracks whether the user is authenticated
      isLoggedIn: auth.isAuthenticated()
    };
  },
  methods: {
    /**
     * Handles successful login emitted by Login component
     */
    onLoginSuccess() {
      this.isLoggedIn = true; // Show TabView after login
    },
    /**
     * Handles logout action from child components
     */
    handleLogout() {
      auth.clearToken();      // Remove authentication token
      this.isLoggedIn = false; // Hide TabView and show Login form
    }
  }
};
</script>

<style scoped>
/* Padding utility for main container */
.p-20 { padding: 20; }
</style>
