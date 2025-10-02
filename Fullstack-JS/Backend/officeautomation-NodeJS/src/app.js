const express = require("express");
const bodyParser = require("body-parser");
const cors = require("cors"); // Enable Cross-Origin Resource Sharing
const swaggerUi = require("swagger-ui-express");
const swaggerDocument = require("./config/swagger");

// Import route modules
const authRoutes = require("./routes/authRoutes");
const cartableRoutes = require("./routes/cartableRoutes");

const app = express();

// Middleware to parse incoming JSON requests
app.use(bodyParser.json());

// CORS middleware must be placed before route definitions
// Allows requests from the Vue frontend (localhost:5173) with credentials
app.use(cors({
  origin: "http://localhost:5173", 
  credentials: true
}));

// API routes
app.use("/api/auth", authRoutes);   // Authentication-related routes
app.use("/api", cartableRoutes);    // Cartable-related routes

// Swagger documentation endpoint
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerDocument));

module.exports = app;
