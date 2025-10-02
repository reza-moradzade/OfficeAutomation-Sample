# OfficeAutomation MAUI Client

A cross-platform **.NET MAUI application** for the OfficeAutomation system.  
This project demonstrates **modular architecture, reusable components, and secure API integration** for mobile and desktop platforms.

---

## Features

### Authentication & State Management
- Login using **OfficeAutomation API** with JWT tokens
- Centralized authentication state via **AuthState**
- Token management and automatic refresh using **TokenService**
- Secure routing based on authentication status
- Login page and UI implemented using **Blazor components** for consistency, not traditional XAML forms

### Cartable & Dashboard
- Cartable page displaying user tasks
- Home dashboard with actionable summaries
- Modular pages (`Home.razor`, `Cartable.razor`, `Login.razor`) for maintainability

### Components & Layout
- Reusable components in `Components` and `Shared`
- Layouts: `MainLayout`, `EmptyLayout`, `NavMenu`
- CSS scoped to components for clean styling
- Responsive design with **Bootstrap 5**
- Supports mobile, tablet, and desktop platforms

### Services & Models
- Strongly-typed **DTOs** (`AuthResponse`, `CartableItemDto`, `CartableResponse`)
- Service interfaces and implementations for **loose coupling**
- Centralized API handling in `AuthService` and `CartableService`

### Best Practices
- Clear separation of **UI, services, and models**
- Centralized API handling and error management
- Prepared for future API versioning and extension
- Cross-platform support with **.NET MAUI** (Android, Windows, macOS)

---

## Getting Started

1. Open `OfficeAutomation.MAUI.sln` in **Visual Studio 2022+** with **.NET MAUI workload installed**.
2. Configure API base URL in `AuthService.cs` or app settings.
3. Build and run the application on **Android, Windows, or macOS**.
4. Log in using your **OfficeAutomation API credentials**.

---

## Notes

- Designed for **learning and demonstration purposes**; production deployment requires additional security and configuration.
- Ensure **OfficeAutomation API server** is running and accessible.
- APK and mobile deployment artifacts are included under the `publish` folder.
