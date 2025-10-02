const swaggerJsdoc = require("swagger-jsdoc");
const path = require("path");

/**
 * Swagger configuration for Office Automation API
 * Generates OpenAPI documentation from JSDoc comments in route files
 */
const options = {
  definition: {
    openapi: "3.0.0",
    info: {
      title: "Office Automation API",
      version: "1.0.0",
      description: "API documentation for Office Automation system",
    },
    servers: [
      { url: "http://localhost:3000/api" }, // Base URL for the API
    ],
    components: {
      securitySchemes: {
        bearerAuth: {
          type: "http",
          scheme: "bearer",
          bearerFormat: "JWT", // Standard JWT format for Authorization header
        },
      },
    },
  },
  // Include all route files for generating Swagger docs
  apis: [path.join(__dirname, "../routes/*.js")],
};

// Generate Swagger specification
const swaggerSpec = swaggerJsdoc(options);

module.exports = swaggerSpec;
