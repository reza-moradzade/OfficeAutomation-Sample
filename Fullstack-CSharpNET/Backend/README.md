# OfficeAutomation API

A secure and modular ASP.NET Core Web API built on top of the OfficeAutomation database.  
This project demonstrates enterprise-grade API practices such as authentication, security, auditing, and task management.  
It is suitable for portfolio, learning, and professional demonstration.

---

## Features

### Authentication & Security
- JWT Authentication with Refresh Tokens
- Role-Based Access Control (RBAC)
- Failed login tracking and brute-force prevention
- Single active session per client type
- Password hashing with salt

### Task & Cartable Management
- Cartable (Inbox) system for workflow simulation
- Task management with file attachments
- Optimized queries for performance

### Auditing & Monitoring
- Audit logs with archiving support
- Logs linked to users for accountability
- Indexed for monitoring and reporting

### Performance & Reliability
- Concurrency control with RowVersion
- Stored procedures for log archiving and session cleanup
- Proper indexing for scalability

---

## API Standards

- RESTful resource-oriented design
- Consistent request/response DTOs
- Standardized error handling
- API versioning-ready structure (/api/v1/…)
- Integrated Swagger / OpenAPI documentation

---

## Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/OfficeAutomation-API.git
   cd OfficeAutomation-API

2. Configure SQL Server connection in `appsettings.json`.

3. Apply database migrations:
   ```bash
   dotnet ef database update

4. Run the API:
   ```bash
   dotnet run --project OfficeAutomation.API

5. Access Swagger UI:
   ```bash
   https://localhost:7169/swagger/index.html

## Notes

- This API is intended for learning and demonstration purposes only, not for production use.
- For production environments:
  - Secure JWT secrets using environment variables or KeyVault
  - Enforce HTTPS and strict CORS policies
  - Store large files externally instead of in the database