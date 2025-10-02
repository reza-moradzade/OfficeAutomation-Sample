# OfficeAutomation Blazor Client

A modern **Blazor WebAssembly / client** for the OfficeAutomation system.  
This project demonstrates **modular architecture, clean component design, and secure integration with the OfficeAutomation API**.

---

## Features

### Authentication & State Management
- Login using **OfficeAutomation API** with JWT tokens
- Centralized authentication state via **AuthState**
- Token management and automatic refresh using **TokenService**
- Secure routing based on authentication status
- Login form and sample components implemented using **MudBlazor** and **Bootstrap** for a modern and reusable UI


### Cartable & Dashboard
- Cartable page displaying user tasks
- Home dashboard with actionable summaries
- Modular pages (`Home.razor`, `Cartable.razor`, `Login.razor`) for maintainability

### Components & Layout
- Reusable components in `Components` and `Shared`
- Layouts: `MainLayout`, `EmptyLayout`, `NavMenu`
- CSS scoped to components for clean styling
- Responsive design using **Bootstrap 5**

### Services & Models
- Strongly-typed **DTOs** (`AuthResponse`, `CartableItemDto`, `CartableResponse`)
- Service interfaces and implementations for **loose coupling**
- API calls centralized in `AuthService` and `CartableService`

### Best Practices
- Clear separation of **UI, services, and models**
- Centralized API handling and error management
- Prepared for future API versioning and extension

---

## Getting Started

1. Open `OfficeAutomation.Blazor.sln` in **Visual Studio 2022+** or **VS Code**.
2. Configure API base URL in `AuthService.cs` or app settings.
3. Build and run the application (**F5** or `dotnet run`).
4. Access the client in your browser:
   https://localhost:{port}/


---

## Notes

- Designed for **learning and demonstration purposes**; production deployment requires further security and configuration.
- Ensure **OfficeAutomation API server** is running and accessible.
- PWA features, offline caching, and service workers are included but optional for demo purposes.
