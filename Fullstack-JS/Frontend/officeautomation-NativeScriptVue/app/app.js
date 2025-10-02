// Entry point for the NativeScript-Vue application
import Vue from 'nativescript-vue';
import App from './components/App'; // Root component of the app

// Create Vue instance and render the root component
new Vue({
  render: (h) => h(App), // Render function for the root component
}).$start(); // Start the NativeScript application
