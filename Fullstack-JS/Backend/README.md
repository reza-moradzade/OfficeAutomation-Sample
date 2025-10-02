# OfficeAutomation Node.js API

A **Node.js / Express API** for the OfficeAutomation system, connected to a **PostgreSQL database**.  
This project demonstrates **modular architecture, clean service-repository separation, and secure authentication**.

---

## Technologies

- **Backend:** Node.js, Express.js  
- **Database:** PostgreSQL  
- **Authentication:** JWT, refresh tokens  
- **Tools:** Postman, Swagger, dotenv, Git  
- **Purpose:** Portfolio, learning, and demonstration of production-grade API patterns  

---

## Key Features

### 1 Authentication & Security
- JWT-based authentication with **access and refresh tokens**  
- Password hashing using **crypto utilities**  
- Role-based access control (RBAC) via middleware  
- Secure route protection with `authMiddleware`  
- Token lifecycle and revocation management  

### 2 API Structure & Modularity
- **Controllers** handle HTTP requests (`AuthController`, `CartableController`)  
- **Services** implement business logic (`AuthService`, `CartableService`)  
- **Repositories** abstract database access (`UserRepository`, `CartableRepository`)  
- **Routes** are separated by module (`authRoutes.js`, `cartableRoutes.js`)  
- Utilities for JWT and crypto functions (`jwtUtil.js`, `cryptoUtil.js`)  

### 3 Database Integration
- PostgreSQL connection configured in `config/database.js`  
- Uses parameterized queries to prevent SQL injection  
- Repository pattern ensures clean separation from Express controllers  

### 4 API Documentation
- Swagger integration configured via `config/swagger.js`  
- Accessible at `/api-docs` when server is running  

### 5 Best Practices
- Centralized **error handling** and middleware  
- Environment configuration via `.env`  
- Modular folder structure for maintainability  
- Prepared for **future scaling and additional modules**  

---

## Getting Started

1. Install dependencies:
2. Configure environment variables in `.env` (database connection, JWT secret, etc.)  
3. Run database migrations or scripts to set up PostgreSQL schema  
4. Start the API server:
5. Access API documentation (Swagger):
   http://localhost:{port}/api-docs

---

## Notes

- Designed for **learning and demonstration purposes**; production deployment requires enhanced security, logging, and monitoring.  
- Always validate user input and sanitize requests.  
- Ensure PostgreSQL server is running and accessible before starting the API.

   