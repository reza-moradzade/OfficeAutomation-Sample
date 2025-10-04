<template>
  <div class="dashboard-layout">
    <Sidebar @menu-changed="handleMenuChange" />
    <MainContent :active-menu="activeMenu" />
  </div>
</template>

<script setup>
// Active menu state
const activeMenu = ref('home')

// Menu change handler function
const handleMenuChange = (menuId) => {
  activeMenu.value = menuId
}
</script>

<style scoped>
.dashboard-layout {
  display: flex;
  height: 100vh;
  overflow: hidden;
}
</style>