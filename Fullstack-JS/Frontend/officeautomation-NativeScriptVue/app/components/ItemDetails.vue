<template>
  <Page>
    <!-- ActionBar at the top with back navigation -->
    <ActionBar>
      <!-- NavigationButton to go back to the previous page -->
      <NavigationButton @tap="$navigateBack()" android.systemIcon="ic_menu_back"/>
      
      <!-- Display the name of the current item -->
      <Label :text="item.name"></Label>
    </ActionBar>

    <!-- Main content layout -->
    <GridLayout>
      <!-- Show item description with top alignment and margin -->
      <Label class="m-10 h3" :text="item.description" verticalAlignment="top"></Label>
    </GridLayout>
  </Page>
</template>

<script>
export default {
  props: ["item"], // Receive "item" object as a prop from parent component
};
</script>

<style scoped lang="scss">
/* Import common theme variables */
@import "@nativescript/theme/scss/variables/blue";

/* Custom styles for this component can be added here */
</style>
