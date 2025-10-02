<template lang="html">
  <Page>
    <!-- ActionBar at the top with page title -->
    <ActionBar>
      <Label text="Cartable"></Label>
    </ActionBar>

    <!-- Main content area using GridLayout -->
    <GridLayout class="page__content">
      
      <!-- Icon for cartable (using emoji instead of FontAwesome) -->
      <Label class="page__content-icon" text="📂"></Label>
      
      <!-- Placeholder or message for the cartable page content -->
      <Label class="page__content-placeholder" :text="message"></Label>
    </GridLayout>
  </Page>
</template>

<script>
export default {
  data() {
    return {
      // Message or placeholder for the cartable page content
      message: "<!-- Browse page content goes here -->"
    };
  }
}
</script>

<style lang="scss" scoped>
/* Import theme variables */
@import "@nativescript/theme/scss/variables/blue";

/* Additional styling for the cartable page can be added here */
</style>
