# OfficeAutomation-Sample

A sample Office Automation system demonstrating a hybrid architecture with Windows Forms, ASP.NET Core API, and modern web clients (Blazor/Vue.js). Designed for portfolio and learning purposes.

## Project Structure

OfficeAutomation-Sample/
├── WinFormsClient/ → Desktop client (Login, Main, Kartable)
│ ├── Services/ → Service layer for API communication
│ ├── Shared/ → DTOs, Models, Helper classes
│ └── WinFormsApp.sln
├── WebAPI/ → ASP.NET Core Web API
│ ├── Controllers/
│ ├── Models/
│ └── WebAPI.sln
├── WebClient/ → Blazor or Vue.js client
│ ├── Pages/
│ ├── Services/
│ └── WebClient.sln
├── Database/ → SQL Scripts, schema, sample data
└── README.md


## Features

- Secure login via API  
- Shared authentication for WinForms and Web clients  
- Inbox (Kartable) page demonstration  
- Multi-layer architecture (UI → Services → API → Database)  
- Example of desktop & web client integration  

## Tech Stack

- **Frontend:** Windows Forms, Blazor (or Vue.js)  
- **Backend:** ASP.NET Core Web API  
- **Database:** SQL Server  
- **Other Tools:** Visual Studio, Git, HttpClient, Entity Framework  

## How to Run

1. Set up the SQL Server database using scripts in `/Database`.  
2. Start the WebAPI project (`WebAPI.sln`)  
3. Run WinFormsClient (`WinFormsClient.sln`)  
4. Run WebClient (`WebClient.sln`)  

## Notes

- This project is for **portfolio and learning purposes**  
- Architecture demonstrates separation of concerns and layered design  
- Designed to show hybrid integration (desktop + web)


