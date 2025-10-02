# OfficeAutomation Vue.js Client

A modern **Vue.js SPA** (Vue 3 + TypeScript + Vite) for the OfficeAutomation system.  
This project demonstrates **modular architecture, component-based design, and secure integration with the Node.js API** using Vue 3 `<script setup>` SFCs.

---

## Features

### Authentication & State Management
- Login via **Node.js OfficeAutomation API** using JWT tokens  
- Centralized authentication state and route guards  
- Automatic token refresh and secure local storage handling  

### Cartable & Dashboard
- Cartable view for displaying user tasks  
- Home dashboard with actionable summaries  
- Modular views (`Home.vue`, `Cartable.vue`, `Login.vue`)  

### Components & Layout
- Reusable components (`Sidebar.vue`, etc.)  
- Responsive layout with CSS and scoped styling  

### Services & API Integration
- Centralized API calls via service modules  
- Error handling and feedback for API responses  

### Best Practices
- Clear separation of **UI, services, and routing**  
- Vue Router for navigation and route protection  
- Prepared for **future scaling and additional modules**  

---

## Getting Started

1. Install dependencies:
2. Configure API base URL in service modules  
3. Run the development server:
4. Access the application in your browser:

---

## Notes

- Designed for **learning and demonstration purposes**; production deployment requires enhanced security, HTTPS, and environment variable management.  
- Ensure **Node.js API server** is running and accessible.  
- Component-based architecture allows easy expansion and maintenance.  
