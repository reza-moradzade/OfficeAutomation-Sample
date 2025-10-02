import { createApp } from 'vue';
import App from './App.vue';
import router from './router';

// Import Bootstrap CSS for styling
import 'bootstrap/dist/css/bootstrap.min.css';

// Create Vue application instance
const app = createApp(App);

// Register router with the application
app.use(router);

// Mount the app to the HTML element with id "app"
app.mount('#app');
