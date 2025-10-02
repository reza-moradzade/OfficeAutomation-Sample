# OfficeAutomation NativeScript-Vue Mobile Client

A cross-platform **NativeScript-Vue mobile application** for the OfficeAutomation system.  
This project demonstrates **modular architecture, component-based UI, and secure integration with the Node.js API and PostgreSQL database**.

---

## Features

### Authentication & Session Management
- Login via **Node.js OfficeAutomation API** using JWT tokens  
- Centralized authentication and token lifecycle management in `auth.js`  
- Secure form handling via `SecureForm.vue`  
- Route protection and session validation for mobile users  

### Cartable & Task Views
- Cartable (Inbox) view with user tasks (`Cartable.vue`, `Items.vue`)  
- Item details and search functionality (`ItemDetails.vue`, `Search.vue`)  
- Modular components for maintainability and reusability  

### Components & UI
- Core app structure in `App.vue`  
- Navigation is **tab-based**, not sidebar-based  
- Responsive and adaptive UI for Android and iOS  
- Custom fonts and styles for mobile optimization (`fa-*`, SCSS files)  
- Modular components for navigation and task interaction  

### Services & API Integration
- Centralized API handling in `auth.js`  
- Strong separation of concerns between UI and API services  
- Compatible with Node.js API and PostgreSQL backend  

### Best Practices
- Clear separation of **UI, services, and components**  
- Component-based architecture ensures maintainability  
- Prepared for **future feature additions** and scaling  
- Tested and previewed using **NativeScript Preview App**  

---

## Getting Started

1. Install dependencies:
2. Configure API base URL in `auth.js`  
3. Run the application in **NativeScript Preview**:
4. Log in using your **OfficeAutomation API credentials** and explore tasks  

---

## Notes

- Designed for **learning and demonstration purposes**; production deployment requires additional security, offline handling, and mobile optimizations  
- Ensure **Node.js API server** is running and accessible  
- Supports **Android and iOS platforms** via NativeScript-Vue  
