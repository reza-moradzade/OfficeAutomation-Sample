# OfficeAutomation WinForms Client

A professional **Windows Forms (WinForms) client** for the OfficeAutomation system.  
This project demonstrates **modular design, clean architecture, and integration with a secure Web API**.

---

## Features

### Authentication & Session
- Login via **OfficeAutomation API** with JWT and refresh tokens
- Session management with **SessionManager**
- Handles login/logout and token lifecycle automatically
- Login form inspired by **WPF style** for a modern interface

### Cartable & Tasks
- Cartable (Inbox) view for user tasks
- Home dashboard with pending actions summary
- Modular views (`HomeView`, `CartableView`) for maintainability

### Architecture & Client
- **Client layer** (`OfficeAutomation.Client`) for API calls
- **Interfaces & Implementations** for loose coupling
- **Models (DTOs)** matching API responses
- **Exception handling** via `ApiException`
- Helper classes for **menu and message management**
- Centralized API handling for easier maintenance

### User Interface
- **MainForm** as container for views
- **BaseContentControl** for reusable UI components
- Forms and views inherit from **BaseContentControl** for consistent layout
- Modular and responsive design

---

## Getting Started

1. Open `OfficeAutomation.WinForms.sln` in **Visual Studio**.
2. Configure API base URL in `ApiClientBase.cs` or app settings.
3. Build and run the application (**F5** or **Ctrl+F5**).
4. Log in using your **OfficeAutomation API credentials**.

---

## Notes

- Designed for **learning and demonstration**; production deployment requires additional security and configuration.
- Ensure **API server** is running and accessible.
- Large file handling and advanced offline support are not included in this demo.
